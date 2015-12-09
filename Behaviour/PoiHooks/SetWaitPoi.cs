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

namespace Tarot.Behaviour.PoiHooks
{
    using System.Threading.Tasks;

    using global::Tarot.Behaviour.PoiHooks.WaitSelect;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class SetWaitPoi
    {
        public static async Task<bool> Main()
        {
            Logger.SendLog("No active FATEs, activating wait mode.");
            switch (TarotSettings.Instance.FateIdleMode)
            {
                case FateIdleMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Main();
                    break;

                case FateIdleMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Main();
                    break;

                case FateIdleMode.GrindMobs:
                    //await GrindMobs.Main();
                    break;

                case FateIdleMode.WaitForFates:
                    //await WaitForFates.Main();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine idle strategy, defaulting to 'Return to Aetheryte'.");
                    await ReturnToAetheryte.Main();
                    break;
            }

            return false;
        }
    }
}