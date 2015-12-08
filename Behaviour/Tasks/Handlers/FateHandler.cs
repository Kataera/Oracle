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

namespace Tarot.Behaviour.Tasks.Handlers
{
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Enums;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Navigation;
    using ff14bot.Settings;

    using global::Tarot.Behaviour.Tasks.Handlers.Fates;
    using global::Tarot.Behaviour.Tasks.Utilities;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;

    internal static class FateHandler
    {
        public static async Task<bool> Task()
        {
            if (Tarot.CurrentFate == null)
            {
                Logger.SendErrorLog("Entered FATE handler without an active FATE assigned.");
                return true;
            }

            await MoveToFate();

            if (WithinFate() && LevelSyncNeeded())
            {
                await LevelSync.Task();
                Logger.SendLog("Synced level to " + Tarot.CurrentFate.MaxLevel + " to participate in FATE.");
            }

            var tarotFateData = Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id);
            var fateTypeNotListed = false;

            switch (tarotFateData.Type)
            {
                case FateType.Kill:
                    await KillFate.Task();
                    break;

                case FateType.Collect:
                    await CollectFate.Task();
                    break;

                case FateType.Escort:
                    await EscortFate.Task();
                    break;

                case FateType.Defence:
                    await DefenceFate.Task();
                    break;

                case FateType.Boss:
                    await BossFate.Task();
                    break;

                case FateType.MegaBoss:
                    await MegaBossFate.Task();
                    break;

                case FateType.Null:
                    Logger.SendErrorLog("Attempting to run a null FATE.");
                    return false;

                default:
                    Logger.SendDebugLog("Cannot determine FATE type, defaulting to Rebornbuddy's classification.");
                    fateTypeNotListed = true;
                    break;
            }

            // Only check FATE icon if there is no listing of the FATE in the database.
            if (fateTypeNotListed)
            {
                switch (Tarot.CurrentFate.Icon)
                {
                    case FateIconType.Battle:
                        await KillFate.Task();
                        break;

                    case FateIconType.KillHandIn:
                        await CollectFate.Task();
                        break;

                    case FateIconType.ProtectNPC:
                        // TODO: Check this is the right way around.
                        await EscortFate.Task();
                        break;

                    case FateIconType.ProtectNPC2:
                        // TODO: Check this is the right way around.
                        await DefenceFate.Task();
                        break;

                    case FateIconType.Boss:
                        await BossFate.Task();
                        break;

                    default:
                        // TODO: Implement blacklist.
                        Logger.SendErrorLog(
                            "FATE has no icon data, it is impossible to determine its type. Blacklisting and moving on.");
                        break;
                }
            }

            return true;
        }

        private static bool LevelSyncNeeded()
        {
            return Tarot.CurrentFate.MaxLevel < Core.Player.ClassLevel && !Core.Player.IsLevelSynced;
        }

        private static bool WithinFate()
        {
            return Tarot.CurrentFate.Location.Distance(Core.Player.Location) < Tarot.CurrentFate.Radius;
        }

        private static async Task<bool> MoveToFate()
        {
            // If we're inside a FATE, cancel.
            if (WithinFate())
            {
                return false;
            }

            // Not using WithinFate method as we want to be within 3/4 of the FATE radius.
            while (Tarot.CurrentFate.Location.Distance(Core.Player.Location) > Tarot.CurrentFate.Radius * 0.75f)
            {
                // Check if the FATE ended while we're moving.
                if (!Tarot.CurrentFate.IsValid || Tarot.CurrentFate.Status == FateStatus.COMPLETE)
                {
                    Logger.SendLog("'" + Tarot.CurrentFate.Name + "' is no longer active.");
                    Navigator.Stop();
                    Navigator.Clear();
                    Navigator.PlayerMover.MoveStop();

                    Poi.Clear("FATE is no longer active");
                    Tarot.CurrentPoi = null;
                    Tarot.CurrentFate = null;

                    return true;
                }

                // Check we're still mounted.
                if (!Core.Player.IsMounted
                    && Core.Player.Distance(Tarot.CurrentFate.Location) > CharacterSettings.Instance.MountDistance)
                {
                    Navigator.PlayerMover.MoveStop();

                    // Exit behaviour if we're in combat.
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }

                Navigator.MoveToPointWithin(Tarot.CurrentFate.Location, 15f, Tarot.CurrentFate.Name);
                await Coroutine.Yield();
            }

            Navigator.Stop();
            Navigator.PlayerMover.MoveStop();
            if (await CommonTasks.CanLand() == CanLandResult.Yes)
            {
                await CommonTasks.Land();
            }

            return true;
        }
    }
}