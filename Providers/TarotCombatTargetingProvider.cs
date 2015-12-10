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

using System.Collections.Generic;
using System.Linq;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

namespace Tarot.Providers
{
    internal struct BattleCharacterWeight
    {
        public BattleCharacter BattleCharacter;

        public double Weight;
    }

    internal class TarotCombatTargetingProvider : ITargetingProvider
    {
        private BattleCharacter[] attackers;

        public TarotCombatTargetingProvider()
        {
            this.IgnoreNpcIds = new HashSet<uint> {1201};
        }

        public HashSet<uint> IgnoreNpcIds { get; set; }

        public List<BattleCharacter> GetObjectsByWeight()
        {
            this.attackers = GameObjectManager.Attackers.ToArray();
            var allBattleCharacters = GameObjectManager.GetObjectsOfType<BattleCharacter>().ToArray();
            var inCombat = Core.Player.InCombat;

            var battleChars = allBattleCharacters.Where(bc => this.Filter(inCombat, bc)).OrderByDescending(this.GetWeight).ToList();

            return battleChars;
        }

        private static bool IsLevelSyncNeeded(GameObject battleCharacter)
        {
            if (battleCharacter.FateId == 0)
            {
                return false;
            }

            return FateManager.GetFateById(battleCharacter.FateId).MaxLevel < Core.Player.ClassLevel;
        }

        private bool Filter(bool inCombat, BattleCharacter battleCharacter)
        {
            if (!battleCharacter.IsValid || battleCharacter.IsDead || !battleCharacter.IsVisible
                || battleCharacter.CurrentHealthPercent <= 0f)
            {
                return false;
            }

            if (!battleCharacter.CanAttack)
            {
                return false;
            }

            if (this.IgnoreNpcIds.Contains(battleCharacter.NpcId))
            {
                return false;
            }

            if (Blacklist.Contains(battleCharacter.ObjectId, BlacklistFlags.Combat))
            {
                return false;
            }

            if (battleCharacter.IsFateGone)
            {
                return false;
            }

            if (IsLevelSyncNeeded(battleCharacter)
                && !FateManager.GetFateById(battleCharacter.FateId).Within2D(battleCharacter.Location))
            {
                return false;
            }

            if (this.attackers.Contains(battleCharacter))
            {
                return true;
            }

            if (!battleCharacter.IsFate && Tarot.CurrentFate != null)
            {
                return false;
            }

            if (Tarot.CurrentFate != null && battleCharacter.FateId != Tarot.CurrentFate.Id)
            {
                return false;
            }

            if (Tarot.CurrentFate == null || !Tarot.CurrentFate.IsValid)
            {
                return false;
            }

            return !inCombat;
        }

        private double GetWeight(BattleCharacter battleCharacter)
        {
            var weight = (battleCharacter.Distance() * -30) + 1800;

            if (battleCharacter.Pointer == Core.Player.PrimaryTargetPtr)
            {
                weight += 150;
            }

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 50;
            }

            if (Tarot.CurrentFate != null && battleCharacter.FateId == Tarot.CurrentFate.Id)
            {
                weight += 210;
            }

            if (this.attackers.Contains(battleCharacter))
            {
                weight += 110;
            }

            if (battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 100f - battleCharacter.CurrentHealthPercent;
            }

            if (!battleCharacter.InCombat)
            {
                weight += 130;
            }

            return weight;
        }
    }
}