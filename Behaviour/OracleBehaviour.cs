using System;
using System.Threading.Tasks;

using Clio.Utilities;

using ff14bot;
using ff14bot.Helpers;

using Oracle.Behaviour.Modes;
using Oracle.Behaviour.Tasks;
using Oracle.Behaviour.Tasks.Utilities;
using Oracle.Enumerations;
using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

using TreeSharp;

namespace Oracle.Behaviour
{
    internal static class OracleBehaviour
    {
        public static Composite Behaviour => CreateBehaviour();

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> Main()
        {
            OracleFateManager.UpdateGameCache();

            if (OracleFateManager.FateDatabase == null)
            {
                await LoadOracleDatabase.Main();
            }

            if (Poi.Current == null)
            {
                Poi.Current = new Poi(Vector3.Zero, PoiType.None);
                return false;
            }

            if (Poi.Current.Type == PoiType.Death || OracleFateManager.DeathFlag || Core.Player.IsDead)
            {
                if (Poi.Current.Type == PoiType.Death || Core.Player.IsDead)
                {
                    Logger.SendLog("We died, attempting to recover.");
                    OracleFateManager.DeathFlag = true;
                }
                else if (OracleFateManager.DeathFlag)
                {
                    await DeathHandler.HandleDeath();
                    OracleFateManager.DeathFlag = false;
                }

                return false;
            }

            switch (MainSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    await FateGrind.Main();
                    break;
                case OracleOperationMode.LevelMode:
                    await Levelling.Main();
                    break;
                case OracleOperationMode.MultiLevelMode:
                    await MultiLevelling.Main();
                    break;
                case OracleOperationMode.SpecificFate:
                    await SpecificFate.Main();
                    break;
                case OracleOperationMode.AtmaGrind:
                    await AtmaGrind.Main();
                    break;
                case OracleOperationMode.AnimusGrind:
                    await AnimusGrind.Main();
                    break;
                case OracleOperationMode.AnimaGrind:
                    await AnimaGrind.Main();
                    break;
                case OracleOperationMode.YokaiWatchGrind:
                    await YokaiWatchGrind.Main();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Always return false to not block the tree.
            return false;
        }
    }
}