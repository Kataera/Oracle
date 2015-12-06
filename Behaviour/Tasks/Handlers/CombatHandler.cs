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
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Buddy.Coroutines;

    using Clio.Utilities;

    using ff14bot;
    using ff14bot.Behavior;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Navigation;

    internal static class CombatHandler
    {
        public static async Task<bool> Task()
        {
            if (Poi.Current.Type != PoiType.Kill)
            {
                foreach (var attacker in GameObjectManager.Attackers)
                {
                    if (!attacker.IsFateGone)
                    {
                        Poi.Clear("Clearing Poi while in combat.");
                        Poi.Current = new Poi(attacker, PoiType.Kill);
                        break;
                    }
                }
            }

            if (Core.Player.CurrentTarget != Poi.Current.Unit)
            {
                Poi.Current.Unit.Target();

                // Give time for the game to update.
                await Coroutine.Sleep(100);
            }

            // Support for Kupo, which will not move in of mobs.
            if (RoutineManager.Current.Name.StartsWith("Kupo", StringComparison.Ordinal))
            {
                if (MovementNeeded())
                {
                    while (MovementNeeded())
                    {
                        Navigator.MoveToPointWithin(
                            Poi.Current.BattleCharacter.Location,
                            Core.Player.CombatReach + RoutineManager.Current.PullRange
                            + Poi.Current.BattleCharacter.CombatReach,
                            "Moving to unit");
                        await Coroutine.Yield();
                    }

                    Navigator.Stop();
                }
            }

            await RoutineManager.Current.CombatBehavior.ExecuteCoroutine();

            // Check if current Poi is dead.
            if (Poi.Current.BattleCharacter != null && Poi.Current.BattleCharacter.IsDead)
            {
                Poi.Clear("Targeted unit is dead, clearing Poi and carrying on!");

                if (GameObjectManager.Attackers.Count >= 1)
                {
                    foreach (var attacker in GameObjectManager.Attackers)
                    {
                        if (!attacker.IsFateGone)
                        {
                            Poi.Current = new Poi(attacker, PoiType.Kill);
                            break;
                        }
                    }
                }
            }

            // Check if current Poi's FATE is gone.
            if (Poi.Current.BattleCharacter != null && Poi.Current.BattleCharacter.IsFateGone)
            {
                Poi.Clear("Target is a FATE mob, and the FATE is gone.");

                if (GameObjectManager.Attackers.Count >= 1)
                {
                    foreach (var attacker in GameObjectManager.Attackers)
                    {
                        if (!attacker.IsFateGone)
                        {
                            Poi.Current = new Poi(attacker, PoiType.Kill);
                            break;
                        }
                    }
                }
            }

            // Check that the BattleCharacter isn't null.
            if (Poi.Current.BattleCharacter == null)
            {
                Poi.Clear("Targeted unit no longer exists, clearing Poi and carrying on!");

                if (GameObjectManager.Attackers.Count >= 1)
                {
                    foreach (var attacker in GameObjectManager.Attackers)
                    {
                        if (!attacker.IsFateGone)
                        {
                            Poi.Current = new Poi(attacker, PoiType.Kill);
                            break;
                        }
                    }
                }
            }

            // Reset Poi back to what it was when we're done.
            if (GameObjectManager.Attackers.Count == 0 && Tarot.CurrentPoi != null)
            {
                Poi.Current = Tarot.CurrentPoi;
            }
            else if (Tarot.CurrentPoi == null && Poi.Current.Type != PoiType.Kill)
            {
                Poi.Current = new Poi(Vector3.Zero, PoiType.None);
            }

            return true;
        }

        private static bool MovementNeeded()
        {
            var minimumDistance = Core.Player.CombatReach + RoutineManager.Current.PullRange
                                  + Poi.Current.BattleCharacter.CombatReach;
            return Core.Player.Location.Distance2D(Poi.Current.BattleCharacter.Location) >= minimumDistance;
        }
    }
}