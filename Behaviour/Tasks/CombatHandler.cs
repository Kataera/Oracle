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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using NeoGaia.ConnectionHandler;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks
{
    internal static class CombatHandler
    {
        private static Stopwatch blacklistCheckTimer;

        public static async Task<bool> Main()
        {
            try
            {
                if (Poi.Current == null)
                {
                    return false;
                }

                if (GameObjectManager.Attackers.All(check => check.IsFateGone) && Poi.Current.Type != PoiType.Kill)
                {
                    return true;
                }

                if (Poi.Current.Type == PoiType.Fate || Poi.Current.Type == PoiType.Wait)
                {
                    Logger.SendLog("Clearing the non-kill point of interest while we're in combat.");
                    Poi.Clear("Character is in combat.");
                }

                // Make sure we don't get stuck attacking a mob that requires us to be level synced.
                else if (LevelSyncNeeded())
                {
                    if (!FateManager.WithinFate)
                    {
                        await MoveIntoFateArea();
                    }

                    await LevelSync.Main();
                }

                await CheckForBlacklist();
                return true;
            }
            catch (Exception exception)
            {
                Logger.SendErrorLog("Memory read error, attempting to mitigate by clearing current Poi.");
                Logger.SendDebugLog("Exception thrown.\n\n" + exception);
                Poi.Clear("Memory read error.");

                return false;
            }
        }

        private static async Task<bool> CheckForBlacklist()
        {
            if (blacklistCheckTimer == null)
            {
                blacklistCheckTimer = Stopwatch.StartNew();
            }

            // Limit checks to every 15 seconds to not send too many requests.
            else if (blacklistCheckTimer.Elapsed < TimeSpan.FromSeconds(15))
            {
                return false;
            }

            if (Poi.Current == null)
            {
                return false;
            }

            var currentBc = Poi.Current.BattleCharacter;
            var navRequest = new List<CanFullyNavigateTarget>
            {
                new CanFullyNavigateTarget {Id = currentBc.ObjectId, Position = currentBc.Location}
            };
            var navResults =
                await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

            if (navResults.FirstOrDefault(result => result.CanNavigate == 0) == null)
            {
                return false;
            }

            Logger.SendDebugLog("Cannot navigate to mob, blacklisting.");
            Blacklist.Add(Poi.Current.BattleCharacter.ObjectId, BlacklistFlags.Combat, TimeSpan.FromSeconds(60),
                "Cannot navigate to mob.");

            blacklistCheckTimer.Restart();
            return true;
        }

        private static bool LevelSyncNeeded()
        {
            if (Poi.Current == null || Poi.Current.BattleCharacter == null || Poi.Current.BattleCharacter.IsDead
                || Poi.Current.BattleCharacter.FateId == 0 || Poi.Current.BattleCharacter.IsFateGone)
            {
                return false;
            }

            var fateId = Poi.Current.BattleCharacter.FateId;
            var fate = FateManager.GetFateById(fateId);

            return Poi.Current != null && Poi.Current.Type == PoiType.Kill && fateId != 0
                   && fate.IsValid
                   && (fate.MaxLevel < Core.Player.ClassLevel)
                   && !Core.Player.IsLevelSynced;
        }

        private static async Task<bool> MoveIntoFateArea()
        {
            var attacker = Poi.Current.BattleCharacter;
            Poi.Clear("Moving back into FATE area.");

            while (Core.Player.Distance2D(FateManager.GetFateById(attacker.FateId).Location)
                   > FateManager.GetFateById(attacker.FateId).Radius * 0.75f)
            {
                Navigator.MoveTo(FateManager.GetFateById(attacker.FateId).Location);
                await Coroutine.Yield();
            }

            Navigator.Stop();
            await
                Coroutine.Wait(
                    TimeSpan.FromSeconds(5),
                    () => FateManager.GetFateById(attacker.FateId).Within2D(attacker.Location));

            // Blacklist the mob if we timed out while waiting.
            if (!FateManager.GetFateById(attacker.FateId).Within2D(attacker.Location))
            {
                Blacklist.Add(
                    attacker.ObjectId,
                    BlacklistFlags.Combat,
                    TimeSpan.FromSeconds(60),
                    "FATE mob is stuck outside FATE area.");
            }

            return true;
        }
    }
}