﻿/*
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

namespace Tarot.Behaviour.Tasks.FateTask
{
    internal static class EscortFate
    {
        private static TimeSpan? movementCooldown;
        private static Stopwatch movementTimer;

        public static async Task<bool> Main()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            var tarotFate = TarotFateManager.FateDatabase.GetFateFromFateData(currentFate);

            if (currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
            {
                ClearFate();
                return true;
            }

            if (movementTimer == null)
            {
                movementTimer = Stopwatch.StartNew();
            }

            if (movementCooldown == null)
            {
                movementCooldown = GetRandomTimeSpan();
            }

            if (currentFate.Status != FateStatus.NOTACTIVE && AnyViableTargets())
            {
                var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
                if (target != null)
                {
                    Poi.Current = new Poi(target, PoiType.Kill);
                }
            }
            else if (currentFate.Status != FateStatus.NOTACTIVE)
            {
                var escortNpc = GameObjectManager.GetObjectByNPCId(tarotFate.NpcId)
                                ?? GameObjectManager.GetObjectsOfType<BattleCharacter>().FirstOrDefault(IsEscortNpc);

                if (escortNpc == null)
                {
                    await MoveToFateCentre();
                    return true;
                }
                await MoveToNpc(escortNpc);
            }

            return true;
        }

        private static bool AnyViableTargets()
        {
            return GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(IsViableTarget).Any();
        }

        private static void ClearFate()
        {
            TarotFateManager.ClearCurrentFate("Current FATE is finished.");
        }

        private static TimeSpan GetRandomTimeSpan()
        {
            return TimeSpan.FromMilliseconds(new Random().Next(500, 1500));
        }

        private static bool IsEscortNpc(BattleCharacter battleCharacter)
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            var tarotFate = TarotFateManager.FateDatabase.GetFateFromFateData(currentFate);

            if (tarotFate.NpcId == battleCharacter.NpcId)
            {
                return true;
            }

            if (!battleCharacter.IsFate)
            {
                return false;
            }

            if (battleCharacter.CanAttack)
            {
                return false;
            }

            if (battleCharacter.FateId != currentFate.Id)
            {
                return false;
            }

            if (!battleCharacter.IsVisible)
            {
                return false;
            }

            if (!battleCharacter.IsTargetable)
            {
                return false;
            }

            return true;
        }

        private static bool IsViableTarget(BattleCharacter target)
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            return target.IsFate && !target.IsFateGone && target.CanAttack && target.FateId == currentFate.Id;
        }

        private static async Task<bool> MoveToFateCentre()
        {
            var currentFate = TarotFateManager.GetCurrentFateData();
            while (Core.Player.Distance2D(currentFate.Location) > currentFate.Radius * 0.2f)
            {
                if (currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
                {
                    Navigator.Stop();
                    ClearFate();
                    return true;
                }

                Navigator.MoveToPointWithin(currentFate.Location, currentFate.Radius * 0.2f, "FATE centre");
                await Coroutine.Yield();
            }

            return true;
        }

        private static async Task<bool> MoveToNpc(GameObject npc)
        {
            if (movementTimer.Elapsed < movementCooldown)
            {
                return true;
            }

            if (!(Core.Player.Distance2D(npc.Location) > 7f))
            {
                return true;
            }

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
                var currentFate = TarotFateManager.GetCurrentFateData();

                if (timeout.Elapsed > TimeSpan.FromSeconds(5))
                {
                    Navigator.PlayerMover.MoveStop();
                    timeout.Reset();
                    return true;
                }

                if (currentFate == null || currentFate.Status == FateStatus.NOTACTIVE || currentFate.Status == FateStatus.COMPLETE)
                {
                    Navigator.PlayerMover.MoveStop();
                    timeout.Reset();
                    ClearFate();
                    return true;
                }

                Navigator.PlayerMover.MoveTowards(location);
                await Coroutine.Yield();
            }

            Navigator.PlayerMover.MoveStop();
            movementTimer.Restart();
            movementCooldown = GetRandomTimeSpan();

            Logger.SendDebugLog("Waiting " + movementCooldown.Value.TotalMilliseconds + "ms before moving again.");
            return true;
        }
    }
}