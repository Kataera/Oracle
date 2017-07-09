using System;
using System.Collections.Generic;

using ff14bot.Managers;

using Oracle.Data.Structs;
using Oracle.Enumerations;
using Oracle.Helpers;

namespace Oracle.Data
{
    [Serializable]
    internal class OracleFateData
    {
        public OracleFateData()
        {
            OracleFateDictionary = new Dictionary<uint, Fate>();
        }

        public OracleFateData(Dictionary<uint, Fate> oracleFateDictionary)
        {
            OracleFateDictionary = oracleFateDictionary;
        }

        public Dictionary<uint, Fate> OracleFateDictionary { get; }

        public void AddFateToDatabase(Fate fate)
        {
            try
            {
                OracleFateDictionary.Add(fate.Id, fate);
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

        public Fate GetFateData(FateData fateData)
        {
            Fate fate;
            try
            {
                if (OracleFateDictionary.TryGetValue(fateData.Id, out fate))
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
                SupportLevel = SupportLevel.None
            };
            Logger.SendDebugLog("Fate with id: '" + fateData.Id + "' not found, flagging as unsupported.");

            return fate;
        }

        public Fate GetFateData(uint id)
        {
            Fate fate;
            try
            {
                if (OracleFateDictionary.TryGetValue(id, out fate))
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
                SupportLevel = SupportLevel.None
            };
            Logger.SendDebugLog("Fate with id: '" + id + "' not found, flagging as unsupported.");

            return fate;
        }
    }
}