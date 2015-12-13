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
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;

using NeoGaia.ConnectionHandler;

using Tarot.Helpers;
using Tarot.Settings;

namespace Tarot.Behaviour.Tasks.WaitTask
{
    internal static class GrindMobs
    {
        public static async Task<bool> Main()
        {
            var target = await GetViableTarget();
            if (target == null)
            {
                return true;
            }

            Logger.SendLog("Selecting '" + target.Name + "' as the next target to kill.");
            Poi.Current = new Poi(target, PoiType.Kill);
            return true;
        }

        private static async Task<BattleCharacter> GetViableTarget()
        {
            var targets = GameObjectManager.GetObjectsOfType<BattleCharacter>().Where(MobFilter).Where(MobWithinRadius);

            var navRequest = targets.Select(target => new CanFullyNavigateTarget {Id = target.ObjectId, Position = target.Location});
            var navResults =
                await Navigator.NavigationProvider.CanFullyNavigateToAsync(navRequest, Core.Player.Location, WorldManager.ZoneId);

            var viableTargets = new Dictionary<BattleCharacter, float>();
            foreach (var result in navResults)
            {
                if (result.CanNavigate == 0)
                {
                    Blacklist.Add(result.Id, BlacklistFlags.Combat, TimeSpan.FromMinutes(15), "Can't navigate to mob.");
                }
                else
                {
                    var battleCharacter = targets.FirstOrDefault(target => target.ObjectId == result.Id);
                    if (battleCharacter != null)
                    {
                        viableTargets.Add(battleCharacter, result.PathLength);
                    }
                }

                await Coroutine.Yield();
            }

            return viableTargets.OrderBy(order => order.Value).FirstOrDefault().Key;
        }

        private static bool MobFilter(BattleCharacter battleCharacter)
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

            if (Blacklist.Contains(battleCharacter.ObjectId, BlacklistFlags.Combat))
            {
                return false;
            }

            if (battleCharacter.IsFateGone)
            {
                return false;
            }

            if (battleCharacter.IsFate)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Contains(battleCharacter))
            {
                return true;
            }

            return true;
        }

        private static bool MobWithinRadius(BattleCharacter battleCharacter)
        {
            return Core.Player.Distance2D(battleCharacter.Location) <= TarotSettings.Instance.GrindMobRadius;
        }
    }
}