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

namespace Tarot.Behaviour.Tasks.Handlers
{
    using System.Threading.Tasks;

    using global::Tarot.Behaviour.Tasks.Handlers.Idles;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;
    using global::Tarot.Settings;

    internal static class IdleHandler
    {
        public static async Task<bool> Task()
        {
            if (Tarot.CurrentFate != null)
            {
                Logger.SendErrorLog("Entered idle handler with an active FATE assigned.");
                return true;
            }

            switch (TarotSettings.Instance.FateIdleMode)
            {
                case (int) FateIdleMode.ReturnToAetheryte:
                    await ReturnToAetheryte.Task();
                    break;

                case (int) FateIdleMode.MoveToWaitLocation:
                    await MoveToWaitLocation.Task();
                    break;

                case (int) FateIdleMode.GrindMobs:
                    await GrindMobs.Task();
                    break;

                case (int) FateIdleMode.WaitForFates:
                    await WaitForFates.Task();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine idle handler strategy, defaulting to 'Return to Aetheryte'.");
                    await ReturnToAetheryte.Task();
                    break;
            }

            return true;
        }
    }
}