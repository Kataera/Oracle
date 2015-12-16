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

using Tarot.Data;
using Tarot.Managers;

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
            var currentFate = TarotFateManager.GetCurrentFateData();
            var blacklistEntry = Blacklist.GetEntry(battleCharacter);

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

            if (blacklistEntry != null)
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

            if (!battleCharacter.IsFate && currentFate != null)
            {
                return false;
            }

            if (currentFate != null && battleCharacter.FateId != currentFate.Id)
            {
                return false;
            }

            if (currentFate == null || !currentFate.IsValid)
            {
                return false;
            }

            return !inCombat;
        }

        private double GetWeight(BattleCharacter battleCharacter)
        {
            var weight = (battleCharacter.Distance() * -30) + 1800;
            var currentFate = TarotFateManager.GetCurrentFateData();
            var tarotFate = new Fate();

            if (currentFate != null)
            {
                tarotFate = TarotFateManager.FateDatabase.GetFateFromFateData(currentFate);
            }

            // If FATE has a preferred target, prioritise it we're out of combat.
            if (tarotFate.PreferredTargetId != null && tarotFate.PreferredTargetId.Contains(battleCharacter.NpcId) && !Core.Player.InCombat)
            {
                weight += 2000;
            }

            // Prefer targets with less mobs around them.
            if (!Core.Player.InCombat)
            {
                weight = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                                          .Aggregate(weight, (current, mob) => current - 2 * (10 / mob.Distance(battleCharacter)));
            }

            if (battleCharacter.Pointer == Core.Player.PrimaryTargetPtr)
            {
                weight += 150;
            }

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 750;
            }

            if (!battleCharacter.TappedByOther)
            {
                weight += 100;
            }

            if (currentFate != null && battleCharacter.FateId == currentFate.Id)
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