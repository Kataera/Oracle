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

namespace Tarot.Behaviour.Tasks.Fates
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ff14bot;
    using ff14bot.Helpers;
    using ff14bot.Managers;
    using ff14bot.Objects;

    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class BossFate
    {
        private static IEnumerable<BattleCharacter> currentFateMobs;

        private static BattleCharacter GetClosestMob()
        {
            PopulateTargetList();
            if (currentFateMobs == null)
            {
                return null;
            }

            // Order by max hp, then the mobs' current hp, then finally by distance.
            return
                currentFateMobs.OrderByDescending(mob => mob.MaxHealth)
                               .ThenByDescending(mob => mob.CurrentHealth)
                               .ThenBy(mob => Core.Me.Distance(mob.Location))
                               .FirstOrDefault(mob => Tarot.CurrentFate.Within2D(mob.Location));
        }

        public static async Task<bool> Main()
        {
            if (Tarot.CurrentFate.Progress < TarotSettings.Instance.BossEngagePercentage)
            {
                if (!TarotSettings.Instance.WaitAtFateForProgress)
                {
                    Logger.SendLog("Current FATE progress reset below minimum level, clearing it and choosing another.");

                    Tarot.CurrentFate = null;
                    Tarot.CurrentPoi = null;
                    Poi.Clear("Current FATE progress reset below minimum level.");
                }
                else
                {
                    Logger.SendLog(
                        "Current FATE progress is too low, waiting for it to reach "
                        + TarotSettings.Instance.BossEngagePercentage + "%.");
                }

                return true;
            }

            var closestMob = GetClosestMob();
            if (closestMob != null)
            {
                Poi.Current = new Poi(closestMob, PoiType.Kill);
            }

            return true;
        }

        private static void PopulateTargetList()
        {
            currentFateMobs =
                GameObjectManager.GetObjectsOfType<BattleCharacter>()
                                 .Where(
                                     mob =>
                                     mob.IsFate && !mob.IsFateGone && mob.CanAttack
                                     && mob.FateId == Tarot.CurrentFate.Id);
        }
    }
}