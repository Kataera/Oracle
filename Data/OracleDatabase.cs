using System;
using System.Collections.Generic;

using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Helpers;

namespace Oracle.Data
{
    internal class OracleDatabase
    {
        private readonly Dictionary<uint, Fate> fateDatabase;

        public OracleDatabase()
        {
            this.fateDatabase = new Dictionary<uint, Fate>();
        }

        public OracleDatabase(Dictionary<uint, Fate> fateDatabase)
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