using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Settings;

namespace Oracle.Managers
{
    internal static class OracleFateManager
    {
        internal static uint CurrentFateId { get; set; }

        internal static bool DeathFlag { get; set; }
        internal static bool DoNotWaitBeforeMovingFlag { get; set; }
        internal static OracleDatabase OracleDatabase { get; set; }
        internal static uint PreviousFateId { get; set; }

        public static async Task<bool> AnyViableFates()
        {
            if (!FateManager.ActiveFates.Any(FateFilter))
            {
                return false;
            }

            await BlacklistBadFates();
            if (FateManager.ActiveFates.Any(FateFilter))
            {
                return true;
            }

            return false;
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
            var oracleFate = OracleDatabase.GetFateFromId(CurrentFateId);

            if (oracleFate.ChainId != 0)
            {
                return true;
            }

            return false;
        }

        public static bool FateFilter(FateData fate)
        {
            var oracleFateData = OracleDatabase.GetFateFromFateData(fate);

            if (oracleFateData.Type == FateType.Boss && !OracleSettings.Instance.BossFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Collect && !OracleSettings.Instance.CollectFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Defence && !OracleSettings.Instance.DefenceFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Escort && !OracleSettings.Instance.EscortFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.Kill && !OracleSettings.Instance.KillFatesEnabled)
            {
                return false;
            }

            if (oracleFateData.Type == FateType.MegaBoss && !OracleSettings.Instance.MegaBossFatesEnabled)
            {
                return false;
            }

            if (OracleSettings.Instance.OracleOperationMode == OracleOperationMode.SpecificFate && !OracleSettings.Instance.SpecificFates.Contains(fate.Id))
            {
                return false;
            }

            if (Blacklist.Contains(fate.Id, BlacklistFlags.Node))
            {
                return false;
            }

            if (OracleSettings.Instance.BlacklistedFates.Contains(fate.Id))
            {
                return false;
            }

            if (OracleSettings.Instance.IgnoreLowDurationUnstartedFates)
            {
                if (Math.Abs(fate.Progress) < 0.5f && fate.TimeLeft < TimeSpan.FromSeconds(OracleSettings.Instance.LowRemainingFateDuration))
                {
                    return false;
                }
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Unsupported)
            {
                return false;
            }

            if (oracleFateData.SupportLevel == FateSupportLevel.Problematic && !OracleSettings.Instance.RunProblematicFates)
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

            if (fate.Level > GetTrueLevel() + OracleSettings.Instance.FateMaximumLevelAbove)
            {
                return false;
            }

            if (fate.Level < GetTrueLevel() - OracleSettings.Instance.FateMinimumLevelBelow)
            {
                return false;
            }

            return true;
        }

        public static bool FateProgressionMet(FateData fate)
        {
            if (OracleDatabase.GetFateFromFateData(fate).Type != FateType.Boss && OracleDatabase.GetFateFromFateData(fate).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.Boss && fate.Progress >= OracleSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.Boss && OracleSettings.Instance.WaitAtBossFateForProgress)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.MegaBoss && fate.Progress >= OracleSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            if (OracleDatabase.GetFateFromFateData(fate).Type == FateType.MegaBoss && OracleSettings.Instance.WaitAtMegaBossFateForProgress)
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

        public static FateData GetCurrentFateData()
        {
            return FateManager.GetFateById(CurrentFateId);
        }

        public static Fate GetCurrentOracleFate()
        {
            return OracleDatabase.GetFateFromId(CurrentFateId);
        }

        public static FateData GetPreviousFateData()
        {
            return FateManager.GetFateById(PreviousFateId);
        }

        public static uint GetTrueLevel()
        {
            var baseClass = GetBaseClass(Core.Player.CurrentJob);
            var trueLevel = Core.Player.Levels.FirstOrDefault(kvp => kvp.Key == baseClass).Value;

            return trueLevel != 0 ? trueLevel : Core.Player.ClassLevel;
        }

        public static bool IsPlayerBeingAttacked()
        {
            return GameObjectManager.Attackers.Any(mob => mob.IsValid && mob.HasTarget && mob.CurrentTargetId == Core.Player.ObjectId && !mob.IsFateGone);
        }

        public static void UpdateGameCache()
        {
            FateManager.Update();
            GameObjectManager.Clear();
            GameObjectManager.Update();
        }

        public static bool ZoneChangeNeeded()
        {
            const ushort dravanianHinterlands = 399;

            if (!OracleSettings.Instance.ZoneChangingEnabled)
            {
                return false;
            }

            if (Core.Player.IsLevelSynced || Core.Player.IsDead)
            {
                return false;
            }

            if (Poi.Current.Type == PoiType.Kill || Poi.Current.Type == PoiType.Fate || CurrentFateId != 0)
            {
                return false;
            }

            uint aetheryteId;
            OracleSettings.Instance.ZoneLevels.TryGetValue(GetTrueLevel(), out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            // Handle Idyllshire.
            if (aetheryteId == 75 && WorldManager.ZoneId == dravanianHinterlands)
            {
                return false;
            }

            return true;
        }
    }
}