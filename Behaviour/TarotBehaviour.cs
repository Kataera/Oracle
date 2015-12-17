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

using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

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
            var currentBc = Poi.Current.BattleCharacter;

            if (currentBc == null)
            {
                return false;
            }

            if (!currentBc.IsValid || currentBc.IsFateGone)
            {
                ClearPoi("Targeted unit is not valid.", false);
                return true;
            }

            // If target is not a FATE mob, nor attacking us.
            if (!currentBc.IsFate && !GameObjectManager.Attackers.Contains(currentBc) && TarotFateManager.CurrentFateId != 0)
            {
                ClearPoi("Targeted unit is not valid.", false);
                return true;
            }

            // If target is not a FATE mob and is tapped by someone else.
            if (!currentBc.IsFate && currentBc.TappedByOther)
            {
                ClearPoi("Targeted unit is not a FATE mob and is tapped by someone else.");
                Blacklist.Add(currentBc, BlacklistFlags.Combat, TimeSpan.FromSeconds(30), "Tapped by another person.");
                Core.Player.ClearTarget();

                if (TarotSettings.Instance.FateWaitMode == FateWaitMode.GrindMobs)
                {
                    var target = await SelectGrindTarget.Main();
                    if (target == null)
                    {
                        return true;
                    }

                    Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
                    Poi.Current = new Poi(target, PoiType.Kill);
                }

                return true;
            }

            // If target is a FATE mob, we need to handle several potential issues.
            if (currentBc.IsFate)
            {
                var fate = FateManager.GetFateById(Poi.Current.BattleCharacter.FateId);

                if (fate == null)
                {
                    return true;
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

        private static async Task<bool> HandleDeath()
        {
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => CommonBehaviors.IsLoading);
            await CommonTasks.HandleLoading();
            await Coroutine.Sleep(TimeSpan.FromSeconds(2));

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
                Logger.SendErrorLog("Can't find requested teleport destination, make sure you've unlocked it.");
                TreeRoot.Stop("Cannot teleport to destination.");
                return false;
            }

            if (!WorldManager.CanTeleport())
            {
                return false;
            }

            var zoneName = WorldManager.AvailableLocations.FirstOrDefault(teleport => teleport.AetheryteId == aetheryteId).Name;
            Logger.SendLog("Character is level " + Core.Player.ClassLevel + ", teleporting to " + zoneName + ".");
            await Teleport.TeleportToAetheryte(aetheryteId);

            if (TarotSettings.Instance.BindHomePoint)
            {
                await BindHomePoint.Main();
            }

            return true;
        }

        private static async Task<bool> Main()
        {
            if (TarotFateManager.TarotDatabase == null)
            {
                await BuildTarotDatabase.Main();
            }

            if (Poi.Current == null)
            {
                Poi.Current = new Poi(Vector3.Zero, PoiType.None);

                return false;
            }

            if (Poi.Current.Type == PoiType.Death || Tarot.DeathFlag)
            {
                if (Poi.Current.Type == PoiType.Death)
                {
                    Logger.SendLog("We died, attempting to recover.");
                    Tarot.DeathFlag = true;
                }
                else if (Tarot.DeathFlag)
                {
                    await HandleDeath();
                    Tarot.DeathFlag = false;
                }

                return false;
            }

            if (TarotSettings.Instance.ChangeZonesEnabled && ZoneChangeNeeded())
            {
                if (Core.Player.InCombat)
                {
                    return false;
                }

                Logger.SendLog("Zone change is needed.");
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