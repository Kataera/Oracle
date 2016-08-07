using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.RemoteWindows;

using NeoGaia.ConnectionHandler;

using Oracle.Data.Fates;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;
using Oracle.Structs;

namespace Oracle.Managers
{
    internal static class OracleFateManager
    {
        internal static uint CurrentFateId { get; set; }

        internal static bool DeathFlag { get; set; }

        internal static bool DoNotWaitBeforeMovingFlag { get; set; }

        internal static FateDatabase FateDatabase { get; set; }

        internal static uint PreviousFateId { get; set; }

        internal static bool ReachedCurrentFate { get; set; } = true;

        public static async Task<bool> AnyViableFates()
        {
            if (!FateManager.ActiveFates.Any(FateFilter))
            {
                return false;
            }

            if (WaitingForChainFate() && !FateManager.ActiveFates.Contains(GetChainFate(PreviousFateId)))
            {
                return false;
            }

            await BlacklistBadFates();
            return FateManager.ActiveFates.Any(FateFilter);
        }

        public static async Task BlacklistBadFates()
        {
            if (!WorldManager.CanFly)
            {
                var navRequest = FateManager.ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var navResult in navResults.Where(result => result.CanNavigate == 0))
                {
                    var fate = FateManager.ActiveFates.FirstOrDefault(result => result.Id == navResult.Id);
                    if (fate == null || Blacklist.Contains(fate.Id))
                    {
                        continue;
                    }

                    Logger.SendDebugLog("'" + fate.Name + "' cannot be navigated to, blacklisting for its remaining duration.");
                    Blacklist.Add(fate.Id, BlacklistFlags.Node, fate.TimeLeft, "Cannot navigate to FATE.");
                }
            }
        }

        public static async Task ClearCurrentFate(string reason)
        {
            Logger.SendLog(reason);
            PreviousFateId = CurrentFateId;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                ClearPoi(reason, false);
            }
        }

        public static async Task ClearCurrentFate(string reason, bool setAsPrevious)
        {
            Logger.SendLog(reason);
            PreviousFateId = setAsPrevious ? CurrentFateId : 0;
            CurrentFateId = 0;

            if (Poi.Current.Type == PoiType.Fate)
            {
                ClearPoi(reason, false);
            }
        }

        public static void ClearPoi(string reason)
        {
            Logger.SendLog(reason);
            Poi.Clear(reason);
        }

        public static void ClearPoi(string reason, bool sendLog)
        {
            if (sendLog)
            {
                Logger.SendLog(reason);
            }

            Poi.Clear(reason);
        }

        public static bool CurrentFateHasChain()
        {
            var oracleFate = FateDatabase.GetFateFromId(CurrentFateId);

            if (oracleFate.ChainId != 0)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> DesyncLevel()
        {
            if (Core.Player.IsLevelSynced)
            {
                ToDoList.LevelSync();
            }

            return true;
        }

        public static bool FateFilter(FateData fate)
        {
            var oracleFateData = FateDatabase.GetFateFromFateData(fate);

            if (oracleFateData.Type == FateType.Boss && !FateSettings.Instance.BossFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Collect && !FateSettings.Instance.CollectFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Defence && !FateSettings.Instance.DefenceFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Escort && !FateSettings.Instance.EscortFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Kill && !FateSettings.Instance.KillFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.MegaBoss && !FateSettings.Instance.MegaBossFatesEnabled)
            {
                return false;
            }

            if (ModeSettings.Instance.OracleOperationMode == OracleOperationMode.SpecificFate && !FateSettings.Instance.SpecificFateList.Contains(fate.Id))
            {
                return false;
            }

            if (Blacklist.Contains(fate.Id, BlacklistFlags.Node))
            {
                return false;
            }

            if (BlacklistSettings.Instance.BlacklistedFates.Contains(fate.Id))
            {
                return false;
            }

            if (FateSettings.Instance.IgnoreLowDuration)
            {
                if (Math.Abs(fate.Progress) < 0.5f && fate.TimeLeft < TimeSpan.FromSeconds(FateSettings.Instance.LowFateDuration))
                {
                    return false;
                }
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Unsupported)
            {
                return false;
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Problematic && !FateSettings.Instance.RunProblematicFates)
            {
                return false;
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.NotInGame)
            {
                return false;
            }

            if (!FateProgressionMet(fate))
            {
                return false;
            }

            if (fate.Level > OracleClassManager.GetTrueLevel() + FateSettings.Instance.FateMaxLevelAbove)
            {
                return false;
            }

            if (fate.Level < OracleClassManager.GetTrueLevel() - FateSettings.Instance.FateMinLevelBelow
                && ModeSettings.Instance.OracleOperationMode == OracleOperationMode.FateGrind)
            {
                return false;
            }

            return true;
        }

        public static bool FateProgressionMet(FateData fate)
        {
            if (FateDatabase.GetFateFromFateData(fate).Type != FateType.Boss && FateDatabase.GetFateFromFateData(fate).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (FateDatabase.GetFateFromFateData(fate).Type == FateType.Boss && fate.Progress >= FateSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (FateDatabase.GetFateFromFateData(fate).Type == FateType.Boss && FateSettings.Instance.WaitAtBossForProgress)
            {
                return true;
            }

            if (FateDatabase.GetFateFromFateData(fate).Type == FateType.MegaBoss && fate.Progress >= FateSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            if (FateDatabase.GetFateFromFateData(fate).Type == FateType.MegaBoss && FateSettings.Instance.WaitAtMegaBossForProgress)
            {
                return true;
            }

            return false;
        }

        public static async Task<Dictionary<FateData, float>> GetActiveFateDistances()
        {
            var activeFates = new Dictionary<FateData, float>();

            if (!WorldManager.CanFly)
            {
                var navRequest = FateManager.ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

                foreach (var result in navResults.Where(result => result.CanNavigate != 0))
                {
                    activeFates.Add(FateManager.GetFateById(result.Id), result.PathLength);
                    await Coroutine.Yield();
                }
            }
            else
            {
                foreach (var fate in FateManager.ActiveFates)
                {
                    activeFates.Add(fate, fate.Location.Distance(Core.Player.Location));
                }
            }

            return activeFates;
        }

        public static async Task<Dictionary<FateData, float>> GetActiveFateDistances(Vector3 location)
        {
            var activeFates = new Dictionary<FateData, float>();

            if (!WorldManager.CanFly)
            {
                var navRequest = FateManager.ActiveFates.Select(fate => new CanFullyNavigateTarget {Id = fate.Id, Position = fate.Location});
                var navResults = await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, location, WorldManager.ZoneId);

                foreach (var result in navResults.Where(result => result.CanNavigate != 0))
                {
                    activeFates.Add(FateManager.GetFateById(result.Id), result.PathLength);
                    await Coroutine.Yield();
                }
            }
            else
            {
                foreach (var fate in FateManager.ActiveFates)
                {
                    activeFates.Add(fate, fate.Location.Distance(location));
                }
            }

            return activeFates;
        }

        public static Tuple<uint, Vector3>[] GetAetheryteIdsForZone(uint zoneId)
        {
            var baseResults = WorldManager.AetheryteIdsForZone(zoneId).ToList();
            foreach (var result in WorldManager.AetheryteIdsForZone(zoneId))
            {
                if (result.Item1 == 19)
                {
                    baseResults.Remove(result);
                    baseResults.Add(new Tuple<uint, Vector3>(19, new Vector3(-168.7496f, 26.1383f, -418.8336f)));
                }
            }

            baseResults.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            return baseResults.ToArray();
        }

        public static ClassJobType GetBaseClass(ClassJobType job)
        {
            switch (job)
            {
                case ClassJobType.Adventurer:
                    return ClassJobType.Adventurer;
                case ClassJobType.Gladiator:
                    return ClassJobType.Gladiator;
                case ClassJobType.Pugilist:
                    return ClassJobType.Pugilist;
                case ClassJobType.Marauder:
                    return ClassJobType.Marauder;
                case ClassJobType.Lancer:
                    return ClassJobType.Lancer;
                case ClassJobType.Archer:
                    return ClassJobType.Archer;
                case ClassJobType.Conjurer:
                    return ClassJobType.Conjurer;
                case ClassJobType.Thaumaturge:
                    return ClassJobType.Thaumaturge;
                case ClassJobType.Carpenter:
                    return ClassJobType.Carpenter;
                case ClassJobType.Blacksmith:
                    return ClassJobType.Blacksmith;
                case ClassJobType.Armorer:
                    return ClassJobType.Armorer;
                case ClassJobType.Goldsmith:
                    return ClassJobType.Goldsmith;
                case ClassJobType.Leatherworker:
                    return ClassJobType.Leatherworker;
                case ClassJobType.Weaver:
                    return ClassJobType.Weaver;
                case ClassJobType.Alchemist:
                    return ClassJobType.Alchemist;
                case ClassJobType.Culinarian:
                    return ClassJobType.Culinarian;
                case ClassJobType.Miner:
                    return ClassJobType.Miner;
                case ClassJobType.Botanist:
                    return ClassJobType.Botanist;
                case ClassJobType.Fisher:
                    return ClassJobType.Fisher;
                case ClassJobType.Paladin:
                    return ClassJobType.Gladiator;
                case ClassJobType.Monk:
                    return ClassJobType.Pugilist;
                case ClassJobType.Warrior:
                    return ClassJobType.Marauder;
                case ClassJobType.Dragoon:
                    return ClassJobType.Lancer;
                case ClassJobType.Bard:
                    return ClassJobType.Archer;
                case ClassJobType.WhiteMage:
                    return ClassJobType.Conjurer;
                case ClassJobType.BlackMage:
                    return ClassJobType.Thaumaturge;
                case ClassJobType.Arcanist:
                    return ClassJobType.Arcanist;
                case ClassJobType.Summoner:
                    return ClassJobType.Arcanist;
                case ClassJobType.Scholar:
                    return ClassJobType.Arcanist;
                case ClassJobType.Rogue:
                    return ClassJobType.Rogue;
                case ClassJobType.Ninja:
                    return ClassJobType.Rogue;
                case ClassJobType.Machinist:
                    return ClassJobType.Machinist;
                case ClassJobType.DarkKnight:
                    return ClassJobType.DarkKnight;
                case ClassJobType.Astrologian:
                    return ClassJobType.Astrologian;
                default:
                    throw new ArgumentOutOfRangeException(nameof(job), job, null);
            }
        }

        public static FateData GetChainFate(FateData fate)
        {
            var oracleFate = FateDatabase.GetFateFromFateData(fate);
            return oracleFate.ChainId != 0 ? FateManager.GetFateById(oracleFate.ChainId) : null;
        }

        public static FateData GetChainFate(uint fateId)
        {
            var oracleFate = FateDatabase.GetFateFromId(fateId);
            return oracleFate.ChainId != 0 ? FateManager.GetFateById(oracleFate.ChainId) : null;
        }

        public static FateData GetCurrentFateData()
        {
            return FateManager.GetFateById(CurrentFateId);
        }

        public static Fate GetCurrentOracleFate()
        {
            return FateDatabase.GetFateFromId(CurrentFateId);
        }

        public static FateData GetPreviousFateData()
        {
            return FateManager.GetFateById(PreviousFateId);
        }

        public static bool IsFateTypeEnabled(Fate oracleFate)
        {
            switch (oracleFate.Type)
            {
                case FateType.Kill:
                    return FateSettings.Instance.KillFatesEnabled;
                case FateType.Collect:
                    return FateSettings.Instance.CollectFatesEnabled;
                case FateType.Escort:
                    return FateSettings.Instance.EscortFatesEnabled;
                case FateType.Defence:
                    return FateSettings.Instance.DefenceFatesEnabled;
                case FateType.Boss:
                    return FateSettings.Instance.BossFatesEnabled;
                case FateType.MegaBoss:
                    return FateSettings.Instance.MegaBossFatesEnabled;
                case FateType.Null:
                    return true;
            }

            return true;
        }

        public static bool IsLevelSyncNeeded(FateData fate)
        {
            if (!fate.IsValid || fate.Status == FateStatus.NOTACTIVE || fate.Status == FateStatus.COMPLETE)
            {
                return false;
            }

            return fate.MaxLevel < OracleClassManager.GetTrueLevel() && !Core.Player.IsLevelSynced && fate.Within2D(Core.Player.Location);
        }

        public static bool IsPlayerBeingAttacked()
        {
            return GameObjectManager.Attackers.Any(mob => mob.IsValid && mob.HasTarget && mob.CurrentTargetId == Core.Player.ObjectId && !mob.IsFateGone);
        }

        public static bool PreviousFateChained()
        {
            if (PreviousFateId == 0)
            {
                return false;
            }

            if (FateDatabase.GetFateFromId(PreviousFateId).ChainId != 0)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> SyncLevel(FateData fate)
        {
            if (!IsLevelSyncNeeded(fate))
            {
                return false;
            }

            ToDoList.LevelSync();
            await Coroutine.Wait(TimeSpan.FromMilliseconds(MainSettings.Instance.ActionDelay), () => Core.Player.IsLevelSynced);

            if (Core.Player.IsLevelSynced)
            {
                Logger.SendLog("Synced to level " + fate.MaxLevel + " to participate in FATE.");
            }

            return true;
        }

        public static void UpdateGameCache()
        {
            FateManager.Update();
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }

        public static bool WaitingForChainFate()
        {
            if (!FateSettings.Instance.WaitForChain)
            {
                return false;
            }

            if (PreviousFateId == 0)
            {
                return false;
            }

            var chainId = FateDatabase.GetFateFromId(PreviousFateId).ChainId;
            if (chainId == 0)
            {
                return false;
            }

            if (!ReachedCurrentFate)
            {
                Logger.SendLog("Not waiting for the next FATE in the chain: we didn't reach the previous FATE.");
                return false;
            }

            var chainOracleFateInfo = FateDatabase.GetFateFromId(chainId);
            if (!IsFateTypeEnabled(chainOracleFateInfo))
            {
                Logger.SendLog("Not waiting for the next FATE in the chain: its type is not enabled.");
                return false;
            }

            if (BlacklistSettings.Instance.BlacklistedFates.Contains(chainId))
            {
                Logger.SendLog("Not waiting for the next FATE in the chain: it is contained in the user blacklist.");
                return false;
            }

            return true;
        }
    }
}