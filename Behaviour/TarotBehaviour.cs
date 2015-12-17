/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;

using Tarot.Behaviour.Tasks;
using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

using TreeSharp;

namespace Tarot.Behaviour
{
    internal static class TarotBehaviour
    {
        public static Composite Behaviour
        {
            get { return CreateBehaviour(); }
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

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> HandleCombat()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            var currentBc = Poi.Current.BattleCharacter;

            if (currentFate == null)
            {
                return false;
            }

            if (currentBc == null)
            {
                return false;
            }

            if (TarotFateManager.CurrentFateId != 0 && currentFate.Status == FateStatus.NOTACTIVE)
            {
                if (TarotFateManager.FateDatabase.GetFateFromFateData(currentFate).Type != FateType.Collect)
                {
                    TarotFateManager.ClearCurrentFate("FATE is no longer active.");
                    return true;
                }
            }

            // If target is not a FATE mob, nor attacking us.
            if (currentBc.IsValid && !currentBc.IsFate && !GameObjectManager.Attackers.Contains(currentBc))
            {
                ClearPoi("Targeted unit is not valid.", false);
                return true;
            }

            // If target is not a FATE mob and is tapped by someone else.
            if (currentBc.TappedByOther && !currentBc.IsFate && TarotSettings.Instance.FateWaitMode == FateWaitMode.GrindMobs)
            {
                ClearPoi("Targeted unit is not a FATE mob and is tapped by someone else.");

                var target = await SelectGrindTarget.Main();
                if (target == null)
                {
                    return true;
                }

                Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
                Poi.Current = new Poi(target, PoiType.Kill);

                return false;
            }

            // If target is a FATE mob, we need to handle several potential issues.
            if (currentBc.IsValid && currentBc.IsFate)
            {
                var fate = FateManager.GetFateById(Poi.Current.BattleCharacter.FateId);

                if (fate == null)
                {
                    return false;
                }

                if (!LevelSync.IsLevelSyncNeeded(fate))
                {
                    return true;
                }

                if (fate.Status != FateStatus.NOTACTIVE)
                {
                    if (fate.Within2D(Core.Player.Location))
                    {
                        await LevelSync.Main(fate);
                    }
                    else if (GameObjectManager.Attackers.Contains(Poi.Current.BattleCharacter))
                    {
                        await MoveToFate.Main(true);
                        await LevelSync.Main(fate);
                    }
                }
            }

            return true;
        }

        private static async Task<bool> HandleFate()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();

            if (currentFate == null)
            {
                return false;
            }

            if (currentFate.Status == FateStatus.NOTACTIVE)
            {
                TarotFateManager.ClearCurrentFate("FATE is no longer active.");
                return false;
            }

            if (Core.Player.Distance(currentFate.Location) > currentFate.Radius)
            {
                await MoveToFate.Main(false);
            }

            if (GameObjectManager.Attackers.Any(mob => !mob.IsFateGone) && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            if (LevelSync.IsLevelSyncNeeded(currentFate))
            {
                await LevelSync.Main(currentFate);
                return true;
            }

            return await FateRunner.Main();
        }

        private static async Task<bool> HandleWait()
        {
            if (GameObjectManager.Attackers.Any(mob => !mob.IsFateGone) && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            if (await TarotFateManager.AnyViableFates())
            {
                ClearPoi("Viable FATE detected.");
                return true;
            }

            return await WaitRunner.Main();
        }

        private static async Task<bool> HandleZoneChange()
        {
            uint aetheryteId = 0;
            TarotSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            var zoneName = WorldManager.AvailableLocations.FirstOrDefault(teleport => teleport.AetheryteId == aetheryteId).Name;
            Logger.SendLog("Character is level " + Core.Player.ClassLevel + ", teleporting to " + zoneName + ".");
            await Teleport.TeleportToAetheryte(aetheryteId);

            return true;
        }

        private static async Task<bool> Main()
        {
            if (Tarot.DeathFlag)
            {
                await Coroutine.Wait(5000, () => CommonBehaviors.IsLoading);
                await CommonTasks.HandleLoading();
                await Coroutine.Sleep(2000);

                Tarot.DeathFlag = false;
            }

            if (TarotFateManager.FateDatabase == null)
            {
                await BuildFateDatabase.Main();
            }

            if (Poi.Current == null)
            {
                return false;
            }

            if (Poi.Current.Type == PoiType.Death)
            {
                Logger.SendLog("We died, attempting to recover.");
                Tarot.DeathFlag = true;
                return false;
            }

            if (TarotSettings.Instance.ChangeZonesEnabled && ZoneChangeNeeded())
            {
                await HandleZoneChange();
                return false;
            }

            switch (Poi.Current.Type)
            {
                case PoiType.Kill:
                    await HandleCombat();
                    return false;
                case PoiType.Fate:
                    await HandleFate();
                    return false;
                case PoiType.Wait:
                    await HandleWait();
                    return false;
            }

            // Always return false to not block the tree.
            return false;
        }

        private static bool ZoneChangeNeeded()
        {
            if (Core.Player.IsLevelSynced || Core.Player.IsDead)
            {
                return false;
            }

            if (Poi.Current.Type == PoiType.Kill || Poi.Current.Type == PoiType.Fate || TarotFateManager.CurrentFateId != 0)
            {
                return false;
            }

            uint aetheryteId = 0;
            TarotSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            return true;
        }
    }
}