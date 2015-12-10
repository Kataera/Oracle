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

using Clio.Utilities;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using Tarot.Helpers;

namespace Tarot.Behaviour.Tasks.Fates
{
    internal static class EscortFate
    {
        public static async Task<bool> Main()
        {
            var target = CombatTargeting.Instance.Provider.GetObjectsByWeight().FirstOrDefault();
            if (target != null)
            {
                Poi.Current = new Poi(target, PoiType.Kill);
                return true;
            }

            // Stay close to the NPC.
            var npc = GameObjectManager.GetObjectsOfType<BattleCharacter>().FirstOrDefault(IsEscortNpc);
            if (npc == null)
            {
                Logger.SendDebugLog("Cannot find escort NPC, defaulting to staying within the centre of the FATE.");
                return true;
            }

            if (Core.Player.Distance2D(npc.Location) > 10f)
            {
                // Find random point within 3 yards of NPC.
                const float radius = 3f;
                const float radiusSquared = radius * radius;
                var xOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * radius);
                var yOffset = Convert.ToSingle(((2 * new Random().NextDouble()) - 1.0) * Math.Sqrt(radiusSquared - (xOffset * xOffset)));
                var location = new Vector3(npc.Location.X + xOffset, npc.Location.Y + yOffset, npc.Location.Z);

                Logger.SendDebugLog("X offset: " + xOffset + ", Y offset: " + yOffset);
                Logger.SendDebugLog("NPC Location: " + npc.Location + ", Moving to: " + location);

                while (Core.Player.Distance2D(location) > 2f)
                {
                    Navigator.PlayerMover.MoveTowards(location);
                    await Coroutine.Yield();
                }

                Navigator.PlayerMover.MoveStop();
            }

            return true;
        }

        private static bool IsEscortNpc(BattleCharacter battleCharacter)
        {
            return battleCharacter.IsFate && !battleCharacter.CanAttack && battleCharacter.FateId == Tarot.CurrentFate.Id;
        }
    }
}