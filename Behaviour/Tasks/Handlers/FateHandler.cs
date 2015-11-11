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

    using ff14bot;
    using ff14bot.Managers;

    using global::Tarot.Behaviour.Tasks.Handlers.Fates;
    using global::Tarot.Behaviour.Tasks.Utilities;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;

    internal static class FateHandler
    {
        public static async Task<bool> Task()
        {
            if (Tarot.CurrentFate == null)
            {
                Logger.SendErrorLog("Entered FATE handler without an active FATE assigned.");
                return true;
            }

            // Check if we need to level sync.
            if (Tarot.CurrentFate.MaxLevel < Core.Player.ClassLevel && !Core.Player.IsLevelSynced)
            {
                await LevelSync.Task();
            }

            var tarotFateData = Tarot.FateDatabase.GetFateWithId(Tarot.CurrentFate.Id);
            var fateTypeNotListed = false;

            switch (tarotFateData.Type)
            {
                case FateType.Kill:
                    await KillFate.Task();
                    break;

                case FateType.Collect:
                    await CollectFate.Task();
                    break;

                case FateType.Escort:
                    await EscortFate.Task();
                    break;

                case FateType.Defence:
                    await DefenceFate.Task();
                    break;

                case FateType.Boss:
                    await BossFate.Task();
                    break;

                case FateType.MegaBoss:
                    await MegaBossFate.Task();
                    break;

                default:
                    Logger.SendDebugLog("Cannot determine FATE type, defaulting to Rebornbuddy's classification.");
                    fateTypeNotListed = true;
                    break;
            }

            // Only check FATE icon if there is no listing of the FATE in the database.
            if (fateTypeNotListed)
            {
                switch (Tarot.CurrentFate.Icon)
                {
                    case FateIconType.Battle:
                        await KillFate.Task();
                        break;

                    case FateIconType.KillHandIn:
                        await CollectFate.Task();
                        break;

                    case FateIconType.ProtectNPC:
                        // TODO: Check this is the right way around.
                        await EscortFate.Task();
                        break;

                    case FateIconType.ProtectNPC2:
                        // TODO: Check this is the right way around.
                        await DefenceFate.Task();
                        break;

                    case FateIconType.Boss:
                        await BossFate.Task();
                        break;

                    default:
                        // TODO: Implement blacklist.
                        Logger.SendErrorLog(
                            "FATE has no icon data, it is impossible to determine its type. Blacklisting and moving on.");
                        break;
                }
            }

            return true;
        }
    }
}