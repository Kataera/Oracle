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

using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Tarot.Enumerations;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.WaitTask
{
    internal static class ReturnToAetheryte
    {
        public static async Task<bool> Main()
        {
            if (Poi.Current.Type != PoiType.Wait)
            {
                return false;
            }

            if (await TarotFateManager.AnyViableFates())
            {
                TarotBehaviour.ClearPoi("Found a FATE.");
                return true;
            }

            if (!(Core.Player.Distance2D(Poi.Current.Location) > 15f))
            {
                return true;
            }

            while (Core.Player.Distance2D(Poi.Current.Location) > 15f)
            {
                // Check if a FATE popped while we're moving.
                if (await TarotFateManager.AnyViableFates())
                {
                    Navigator.Stop();
                    TarotBehaviour.ClearPoi("Found a FATE.");
                    return true;
                }

                // Check we're still mounted.
                if (!Core.Player.IsMounted
                    && Core.Player.Distance(Poi.Current.Location) > CharacterSettings.Instance.MountDistance)
                {
                    Navigator.PlayerMover.MoveStop();

                    // Exit behaviour if we're in combat.
                    if (Core.Player.InCombat)
                    {
                        return false;
                    }

                    await CommonBehaviors.CreateMountBehavior().ExecuteCoroutine();
                }

                Navigator.MoveToPointWithin(Poi.Current.Location, 15f, "Moving to Aetheryte");
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();

            return true;
        }

        private static bool ProgressedEnough(FateData fate)
        {
            if (TarotSettings.Instance.WaitAtFateForProgress)
            {
                return true;
            }

            if (TarotFateManager.FateDatabase.GetFateWithId(fate.Id).Type != FateType.Boss
                && TarotFateManager.FateDatabase.GetFateWithId(fate.Id).Type != FateType.MegaBoss)
            {
                return true;
            }

            if (TarotFateManager.FateDatabase.GetFateWithId(fate.Id).Type == FateType.Boss
                && fate.Progress >= TarotSettings.Instance.BossEngagePercentage)
            {
                return true;
            }

            if (TarotFateManager.FateDatabase.GetFateWithId(fate.Id).Type == FateType.MegaBoss
                && fate.Progress >= TarotSettings.Instance.MegaBossEngagePercentage)
            {
                return true;
            }

            return false;
        }
    }
}