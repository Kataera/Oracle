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

using ff14bot.Managers;

using Tarot.Enumerations;
using Tarot.Helpers;

namespace Tarot.Data
{
    internal class FateDatabase
    {
        private readonly Dictionary<uint, Fate> fateDatabase;

        public FateDatabase()
        {
            this.fateDatabase = new Dictionary<uint, Fate>();
        }

        public FateDatabase(Dictionary<uint, Fate> fateDatabase)
        {
            this.fateDatabase = fateDatabase;
        }

        public void AddFateToDatabase(Fate fate)
        {
            try
            {
                this.fateDatabase.Add(fate.Id, fate);
            }
            catch (ArgumentNullException exception)
            {
                Logger.SendErrorLog("Error adding FATE to the database.");
                Logger.SendDebugLog("ArgumentNullException thrown:\n\n" + exception + "\n");
            }
            catch (ArgumentException exception)
            {
                Logger.SendErrorLog("Error adding FATE to the database.");
                Logger.SendDebugLog("ArgumentException thrown:\n\n" + exception + "\n");
            }
        }

        public Fate GetFateFromFateData(FateData fateData)
        {
            Fate fate;
            try
            {
                if (this.fateDatabase.TryGetValue(fateData.Id, out fate))
                {
                    return fate;
                }
            }
            catch (ArgumentNullException exception)
            {
                Logger.SendErrorLog("Error looking up FATE in the database.");
                Logger.SendDebugLog("ArgumentNullException thrown:\n\n" + exception + "\n");
            }

            // Create a null fate with Unsupported flag if we can't find it.
            fate = new Fate {SupportLevel = FateSupportLevel.Unsupported};
            Logger.SendDebugLog("Fate with id: '" + fateData.Id + "' not found, flagging as unsupported.");

            return fate;
        }

        public Fate GetFateFromId(uint id)
        {
            Fate fate;
            try
            {
                if (this.fateDatabase.TryGetValue(id, out fate))
                {
                    return fate;
                }
            }
            catch (ArgumentNullException exception)
            {
                Logger.SendErrorLog("Error looking up FATE in the database.");
                Logger.SendDebugLog("ArgumentNullException thrown:\n\n" + exception + "\n");
            }

            // Create a null fate with Unsupported flag if we can't find it.
            fate = new Fate {SupportLevel = FateSupportLevel.Unsupported};
            Logger.SendDebugLog("Fate with id: '" + id + "' not found, flagging as unsupported.");

            return fate;
        }
    }
}