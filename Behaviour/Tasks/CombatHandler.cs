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

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;

using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks
{
    internal static class CombatHandler
    {
        public static async Task<bool> Main()
        {
            if (Poi.Current != null && GameObjectManager.Attackers.Any())
            {
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
            }

            return true;
        }

        private static bool LevelSyncNeeded()
        {
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
                    attacker,
                    BlacklistFlags.Combat,
                    TimeSpan.FromSeconds(60),
                    "FATE mob is stuck outside FATE area.");
            }

            return true;
        }
    }
}