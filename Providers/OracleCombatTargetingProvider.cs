/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System.Collections.Generic;
using System.Linq;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.NeoProfiles;
using ff14bot.Objects;

using Oracle.Data;
using Oracle.Enumerations;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Providers
{
    internal class OracleCombatTargetingProvider : ITargetingProvider
    {
        public List<BattleCharacter> GetObjectsByWeight()
        {
            var allTargets = GameObjectManager.GetObjectsOfType<BattleCharacter>().ToArray();
            if (ReadyToTurnIn())
            {
                return new List<BattleCharacter>();
            }

            return allTargets.Where(bc => Filter(Core.Player.InCombat, bc)).OrderByDescending(GetWeight).ToList();
        }

        private static bool Filter(bool inCombat, BattleCharacter battleCharacter)
        {
            var currentFate = OracleFateManager.GetCurrentFateData();
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

            if (GameObjectManager.Attackers.Contains(battleCharacter))
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

        private static double GetWeight(BattleCharacter battleCharacter)
        {
            var weight = 1800 - (battleCharacter.Distance(Core.Player) * 50);
            var currentFate = OracleFateManager.GetCurrentFateData();
            var oracleFate = new Fate();

            if (currentFate != null)
            {
                oracleFate = OracleFateManager.OracleDatabase.GetFateFromFateData(currentFate);
            }

            // If FATE has a preferred target, prioritise it if we're out of combat.
            if (oracleFate.PreferredTargetId != null && oracleFate.PreferredTargetId.Contains(battleCharacter.NpcId)
                && !Core.Player.InCombat)
            {
                weight += 2000;
            }

            if (battleCharacter.Pointer == Core.Player.PrimaryTargetPtr)
            {
                weight += 350;
            }

            if (battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Core.Player.ObjectId)
            {
                weight += 750;
            }

            if (Chocobo.Object != null && battleCharacter.HasTarget && battleCharacter.CurrentTargetId == Chocobo.Object.ObjectId)
            {
                weight += 500;
            }

            if (!battleCharacter.TappedByOther)
            {
                weight += 100;
            }

            if (currentFate != null && battleCharacter.FateId == currentFate.Id)
            {
                weight += 210;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
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

        private static bool IsLevelSyncNeeded(GameObject battleCharacter)
        {
            if (battleCharacter.FateId == 0)
            {
                return false;
            }

            return FateManager.GetFateById(battleCharacter.FateId).MaxLevel < Core.Player.ClassLevel;
        }

        private static bool ReadyToTurnIn()
        {
            if (Core.Player.InCombat)
            {
                return false;
            }

            var currentFate = OracleFateManager.GetCurrentFateData();
            if (currentFate == null)
            {
                return false;
            }

            var oracleFate = OracleFateManager.OracleDatabase.GetFateFromId(currentFate.Id);
            if (oracleFate.Type != FateType.Collect)
            {
                return false;
            }

            if (currentFate.Status == FateStatus.NOTACTIVE)
            {
                return false;
            }

            var fateItemCount = ConditionParser.ItemCount(oracleFate.ItemId);
            if (fateItemCount >= OracleSettings.Instance.CollectFateTurnInAtAmount)
            {
                return true;
            }

            return false;
        }
    }
}