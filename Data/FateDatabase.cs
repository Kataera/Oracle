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

namespace Tarot.Data
{
    using System;
    using System.Collections.Generic;

    using global::Tarot.Data.FateTypes;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;

    internal class FateDatabase
    {
        private readonly Dictionary<int, Fate> fateDatabase;

        public FateDatabase()
        {
            this.fateDatabase = new Dictionary<int, Fate>();
        }

        public FateDatabase(Dictionary<int, Fate> fateDatabase)
        {
            this.fateDatabase = fateDatabase;
        }

        public Fate GetFateWithId(int id)
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

            // Create empty kill fate with Unsupported flag.
            fate = new Kill { SupportLevel = (int) FateSupportLevel.Unsupported };
            Logger.SendDebugLog("Fate with id: '" + id + "' not found, flagging as unsupported.");

            return fate;
        }

        public void AddFateToDatabase(Fate fate)
        {
            if (fate == null)
            {
                Logger.SendDebugLog("Cannot add passed FATE to database, it is null.");
                return;
            }

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
    }
}