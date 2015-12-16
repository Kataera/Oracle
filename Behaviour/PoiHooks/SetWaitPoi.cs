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

using System.Threading.Tasks;

using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;

using Tarot.Behaviour.PoiHooks.WaitSelect;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Managers;
using Tarot.Settings;

namespace Tarot.Behaviour.PoiHooks
{
    internal static class SetWaitPoi
    {
        public static async Task<bool> Main()
        {
            if (CommonBehaviors.IsLoading)
            {
                return false;
            }

            if (ZoneChangeNeeded())
            {
                return false;
            }

            TarotFateManager.SetDoNotWaitFlag(false);
            switch (TarotSettings.Instance.FateWaitMode)
            {
                case FateWaitMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Main();
                    break;

                case FateWaitMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Main();
                    break;

                case FateWaitMode.GrindMobs:
                    await GrindMobs.Main();
                    break;

                case FateWaitMode.WaitInPlace:
                    await WaitInPlace.Main();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine idle strategy, defaulting to 'Return to Aetheryte'.");
                    await ReturnToAetheryte.Main();
                    break;
            }

            return false;
        }

        private static bool ZoneChangeNeeded()
        {
            uint aetheryteId = 0;
            TarotSettings.Instance.ZoneLevels.TryGetValue(Core.Player.ClassLevel, out aetheryteId);

            if (aetheryteId == 0 || !WorldManager.HasAetheryteId(aetheryteId))
            {
                return false;
            }

            if (WorldManager.GetZoneForAetheryteId(aetheryteId) == WorldManager.ZoneId)
            {
                return false;
            }

            return true;
        }
    }
}