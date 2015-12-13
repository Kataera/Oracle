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
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Settings;

using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.Utilities
{
    internal static class MoveToFate
    {
        public static async Task<bool> Main(bool ignoreCombat)
        {
            if (!ignoreCombat && GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                return false;
            }

            if (!ignoreCombat && TarotSettings.Instance.TeleportIfQuicker)
            {
                if (await Teleport.FasterToTeleport(TarotFateManager.CurrentFate))
                {
                    Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                    await Teleport.TeleportToClosestAetheryte(TarotFateManager.CurrentFate);
                }
            }

            if (!ignoreCombat && IsMountNeeded() && !Core.Player.IsMounted)
            {
                while (!await MountUp())
                {
                    await Coroutine.Yield();
                }
            }

            await Move();
            return true;
        }

        private static bool IsMountNeeded()
        {
            if (TarotFateManager.CurrentFate == null || !TarotFateManager.CurrentFate.IsValid)
            {
                return false;
            }

            var distanceToFateBoundary = Core.Player.Distance(TarotFateManager.CurrentFate.Location) - TarotFateManager.CurrentFate.Radius;
            return distanceToFateBoundary > CharacterSettings.Instance.MountDistance;
        }

        private static async Task<bool> MountUp()
        {
            if (!Actionmanager.AvailableMounts.Any())
            {
                Logger.SendDebugLog("Character does not have any mount available, skipping mount task.");
                return true;
            }

            while (!Core.Player.IsMounted)
            {
                if (GameObjectManager.Attackers.Any())
                {
                    return false;
                }

                if (Actionmanager.CanMount == 0)
                {
                    Actionmanager.Mount();
                }
                await Coroutine.Yield();
            }

            return true;
        }

        private static async Task<bool> Move()
        {
            var fate = TarotFateManager.CurrentFate;

            if (WorldManager.CanFly && PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                while (!FateManager.WithinFate)
                {
                    if (!fate.IsValid || fate.Status == FateStatus.COMPLETE)
                    {
                        Logger.SendLog("'" + fate.Name + "' ended before we got there.");
                        TarotFateManager.ClearCurrentFate("FATE has ended.", false);
                        TarotFateManager.DoNotWaitBeforeMoving = true;

                        Navigator.Stop();
                        return true;
                    }

                    Navigator.MoveToPointWithin(fate.Location, fate.Radius * 0.5f, fate.Name);
                    await Coroutine.Yield();
                }
            }
            else
            {
                while (Core.Player.Distance(fate.Location) > fate.Radius * 0.75f)
                {
                    if (!fate.IsValid || fate.Status == FateStatus.COMPLETE)
                    {
                        Logger.SendLog("'" + fate.Name + "' ended before we got there.");
                        TarotFateManager.ClearCurrentFate("FATE has ended.", false);

                        Navigator.Stop();
                        return true;
                    }

                    Navigator.MoveToPointWithin(fate.Location, fate.Radius * 0.5f, fate.Name);
                    await Coroutine.Yield();
                }
            }

            Navigator.Stop();
            return true;
        }
    }
}