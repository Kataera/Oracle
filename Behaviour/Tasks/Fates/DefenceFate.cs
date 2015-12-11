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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using Clio.Utilities;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Tarot.Helpers;
using Tarot.Managers;

namespace Tarot.Behaviour.Tasks.Fates
{
    internal static class DefenceFate
    {
        private static int movementCooldown;
        private static Stopwatch movementTimer;

        public static async Task<bool> Main()
        {
            if (TarotFateManager.CurrentFate.Status == FateStatus.COMPLETE)
            {
                ClearFate();
                return true;
            }

            if (movementTimer == null)
            {
                movementTimer = Stopwatch.StartNew();
            }

            if (movementCooldown == 0)
            {
                movementCooldown = new Random().Next(500, 1500);
            }

            if (AnyViableTargets())
            {
                var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
                if (target != null)
                {
                    Poi.Current = new Poi(target, PoiType.Kill);
                }
            }
            else
            {
                var escortNpc = GameObjectManager.GetObjectsOfType<BattleCharacter>().FirstOrDefault(IsDefenceNpc);
                if (escortNpc == null)
                {
                    Logger.SendDebugLog("Cannot find any defence NPC, defaulting to staying within the centre of the FATE.");
                    while (Core.Player.Distance2D(TarotFateManager.CurrentFate.Location) > TarotFateManager.CurrentFate.Radius * 0.2f)
                    {
                        Navigator.MoveToPointWithin(TarotFateManager.CurrentFate.Location, TarotFateManager.CurrentFate.Radius * 0.2f,
                            "FATE centre.");
                        await Coroutine.Yield();
                    }

                    return true;
                }

                // Don't spam movement.
                if (movementTimer.Elapsed < TimeSpan.FromMilliseconds(Convert.ToDouble(movementCooldown)))
                {
                    return true;
                }

                if (!(Core.Player.Distance2D(escortNpc.Location) > 7f))
                {
                    return true;
                }

                await MoveToNpc(escortNpc);

                movementCooldown = new Random().Next(500, 1500);
                Logger.SendDebugLog("Movement successful, waiting " + movementCooldown + "ms before moving again.");
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static void ClearFate()
        {
            Logger.SendLog("Current FATE is finished.");
            Poi.Clear("Current FATE is finished.");
            TarotFateManager.PreviousFate = TarotFateManager.CurrentFate;
            TarotFateManager.CurrentFate = null;
        }

        private static bool IsDefenceNpc(BattleCharacter battleCharacter)
        {
            return battleCharacter.IsFate && !battleCharacter.CanAttack && battleCharacter.FateId == TarotFateManager.CurrentFate.Id;
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == TarotFateManager.CurrentFate.Id;
        }

        private static async Task<bool> MoveToNpc(GameObject npc)
        {
            // Find random point within 3 yards of NPC.
            const float radius = 3f;
            const float radiusSquared = radius * radius;
            var xOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * radius);
            var yOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * Math.Sqrt(radiusSquared - (xOffset * xOffset)));
            var location = new Vector3(npc.Location.X + xOffset, npc.Location.Y + yOffset, npc.Location.Z);

            Logger.SendDebugLog("NPC Location: " + npc.Location + ", Moving to: " + location);
            var timeout = new Stopwatch();
            timeout.Start();

            while (Core.Player.Distance2D(location) > 1f)
            {
                if (timeout.Elapsed > TimeSpan.FromSeconds(5))
                {
                    timeout.Reset();
                    await MoveToNpc(npc);

                    return true;
                }

                Navigator.PlayerMover.MoveTowards(location);
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();
            movementTimer.Restart();

            return true;
        }
    }
}