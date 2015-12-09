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

namespace Tarot.Behaviour.Tasks
{
    using System.Linq;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Enums;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Navigation;
    using ff14bot.Settings;

    using global::Tarot.Behaviour.Tasks.Fates;
    using global::Tarot.Behaviour.Tasks.Utilities;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class FateHandler
    {
        private static bool FateHasEnoughProgress()
        {
            if (Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id).Type == FateType.Boss
                && Tarot.CurrentFate.Progress < TarotSettings.Instance.BossEngagePercentage)
            {
                return false;
            }

            if (Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id).Type == FateType.MegaBoss
                && Tarot.CurrentFate.Progress < TarotSettings.Instance.MegaBossEngagePercentage)
            {
                return false;
            }

            return true;
        }

        private static async Task<bool> Land()
        {
            if (FateHasEnoughProgress())
            {
                if (WorldManager.CanFly && PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
                {
                    if (await CommonTasks.CanLand() == CanLandResult.Yes)
                    {
                        await CommonTasks.Land();
                    }
                    else
                    {
                        var moveResult = Navigator.MoveTo(Tarot.CurrentFate.Location, "FATE centre.");
                        while (moveResult != MoveResult.Done)
                        {
                            moveResult = Navigator.MoveTo(Tarot.CurrentFate.Location, "FATE centre.");
                            await Coroutine.Yield();
                        }
                    }
                }
            }

            return true;
        }

        private static bool LevelSyncNeeded()
        {
            return Tarot.CurrentFate.MaxLevel < Core.Player.ClassLevel && !Core.Player.IsLevelSynced;
        }

        public static async Task<bool> Main()
        {
            if (Poi.Current != null && Poi.Current.Type == PoiType.Fate && Tarot.CurrentFate != null)
            {
                if (Tarot.CurrentFate == null)
                {
                    Logger.SendErrorLog("Entered FATE handler without an active FATE assigned.");
                    return true;
                }

                await MoveToFate();

                if (WithinFate() && LevelSyncNeeded())
                {
                    await LevelSync.Main();
                    Logger.SendLog("Synced level to " + Tarot.CurrentFate.MaxLevel + " to participate in FATE.");
                }

                var tarotFateData = Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id);
                var fateTypeNotListed = false;

                switch (tarotFateData.Type)
                {
                    case FateType.Kill:
                        await KillFate.Main();
                        break;

                    case FateType.Collect:
                        await CollectFate.Main();
                        break;

                    case FateType.Escort:
                        await EscortFate.Main();
                        break;

                    case FateType.Defence:
                        await DefenceFate.Main();
                        break;

                    case FateType.Boss:
                        await BossFate.Main();
                        break;

                    case FateType.MegaBoss:
                        await MegaBossFate.Main();
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
                            await KillFate.Main();
                            break;

                        case FateIconType.KillHandIn:
                            await CollectFate.Main();
                            break;

                        case FateIconType.ProtectNPC:
                            // TODO: Check this is the right way around.
                            await EscortFate.Main();
                            break;

                        case FateIconType.ProtectNPC2:
                            // TODO: Check this is the right way around.
                            await DefenceFate.Main();
                            break;

                        case FateIconType.Boss:
                            await BossFate.Main();
                            break;

                        default:
                            // TODO: Implement blacklist.
                            Logger.SendErrorLog(
                                "FATE has no icon data, it is impossible to determine its type. Blacklisting and moving on.");
                            break;
                    }
                }
            }
            return true;
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
            await Land();

            return true;
        }

        private static bool WithinFate()
        {
            return Tarot.CurrentFate.Location.Distance(Core.Player.Location) < Tarot.CurrentFate.Radius;
        }
    }
}