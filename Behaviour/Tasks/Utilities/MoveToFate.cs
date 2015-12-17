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
            var currentFate = TarotFateManager.GetCurrentFateData();

            if (!ignoreCombat && GameObjectManager.Attackers.Any() && !Core.Player.IsMounted)
            {
                return false;
            }

            if (!ignoreCombat && TarotSettings.Instance.TeleportIfQuicker && currentFate.IsValid)
            {
                if (await Teleport.FasterToTeleport(currentFate))
                {
                    Logger.SendLog("Teleporting to the closest aetheryte crystal to the FATE.");
                    await Teleport.TeleportToClosestAetheryte(currentFate);
                }
            }

            if (!ignoreCombat && IsMountNeeded() && !Core.Player.IsMounted && currentFate.IsValid)
            {
                while (!await MountUp())
                {
                    await Coroutine.Yield();
                }
            }

            await Move(ignoreCombat);
            return true;
        }

        private static void ClearFate(FateData fate)
        {
            TarotFateManager.SetDoNotWaitFlag(true);
            Logger.SendLog("'" + fate.Name + "' ended before we got there.");
            TarotFateManager.ClearCurrentFate("FATE has ended.", false);
            Navigator.Stop();
        }

        private static bool IsMountNeeded()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();

            if (currentFate == null || !currentFate.IsValid)
            {
                return false;
            }

            var distanceToFateBoundary = Core.Player.Distance(currentFate.Location) - currentFate.Radius;
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

        private static async Task<bool> Move(bool ignoreCombat)
        {
            var currentFate = TarotFateManager.GetCurrentFateData();

            if (currentFate == null || !currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE
                || currentFate.Status == FateStatus.NOTACTIVE)
            {
                ClearFate(currentFate);
                return true;
            }

            if (WorldManager.CanFly && PluginManager.GetEnabledPlugins().Contains("EnableFlight"))
            {
                while (!FateManager.WithinFate)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        ClearFate(currentFate);
                        Navigator.Stop();
                        return true;
                    }

                    if (!Core.Player.IsMounted && IsMountNeeded() && Actionmanager.AvailableMounts.Any())
                    {
                        Navigator.Stop();
                        if (!ignoreCombat && Core.Player.InCombat)
                        {
                            return true;
                        }

                        await MountUp();
                    }

                    Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.5f, currentFate.Name);
                    await Coroutine.Yield();
                }
            }
            else
            {
                while (Core.Player.Distance(currentFate.Location) > currentFate.Radius * 0.75f)
                {
                    if (!currentFate.IsValid || currentFate.Status == FateStatus.COMPLETE || currentFate.Status == FateStatus.NOTACTIVE)
                    {
                        ClearFate(currentFate);
                        Navigator.Stop();
                        return true;
                    }

                    if (!Core.Player.IsMounted && IsMountNeeded() && Actionmanager.AvailableMounts.Any())
                    {
                        Navigator.Stop();
                        if (!ignoreCombat && Core.Player.InCombat)
                        {
                            return true;
                        }

                        await MountUp();
                    }

                    Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.5f, currentFate.Name);
                    await Coroutine.Yield();
                }
            }

            Navigator.Stop();
            return true;
        }
    }
}