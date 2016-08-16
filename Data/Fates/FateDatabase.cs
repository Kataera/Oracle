using System;
using System.Collections.Generic;

using ff14bot.Managers;

using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Structs;

namespace Oracle.Data.Fates
{
    [Serializable]
    internal class FateDatabase
    {
        public FateDatabase()
        {
            FateDictionary = new Dictionary<uint, Fate>();
        }

        public FateDatabase(Dictionary<uint, Fate> fateDictionary)
        {
            FateDictionary = fateDictionary;
        }

        public Dictionary<uint, Fate> FateDictionary { get; }

        public void AddFateToDatabase(Fate fate)
        {
            try
            {
                FateDictionary.Add(fate.Id, fate);
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
                if (FateDictionary.TryGetValue(fateData.Id, out fate))
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
            fate = new Fate
            {
                SupportLevel = FateSupportLevel.Unsupported
            };
            Logger.SendDebugLog("Fate with id: '" + fateData.Id + "' not found, flagging as unsupported.");

            return fate;
        }

        public Fate GetFateFromId(uint id)
        {
            Fate fate;
            try
            {
                if (FateDictionary.TryGetValue(id, out fate))
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
            fate = new Fate
            {
                SupportLevel = FateSupportLevel.Unsupported
            };
            Logger.SendDebugLog("Fate with id: '" + id + "' not found, flagging as unsupported.");

            return fate;
        }
    }
}