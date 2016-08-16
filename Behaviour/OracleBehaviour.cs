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
            OracleFateManager.ForceUpdateGameCache();

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

            await ChocoboHandler.HandleChocobo();

            if (Poi.Current.Type == PoiType.Kill)
            {
                await CombatHandler.HandleCombat();
                return false;
            }

            switch (ModeSettings.Instance.OracleOperationMode)
            {
                case OracleOperationMode.FateGrind:
                    await FateGrind.HandleFateGrind();
                    break;
                case OracleOperationMode.LevelMode:
                    await Levelling.HandleLevelling();
                    break;
                case OracleOperationMode.MultiLevelMode:
                    await MultiLevelling.HandleMultiLevelling();
                    break;
                case OracleOperationMode.SpecificFates:
                    await SpecificFates.HandleSpecificFates();
                    break;
                case OracleOperationMode.AtmaGrind:
                    await AtmaGrind.HandleAtmaGrind();
                    break;
                case OracleOperationMode.AnimusGrind:
                    await AnimusGrind.HandleAnimusGrind();
                    break;
                case OracleOperationMode.AnimaGrind:
                    await AnimaGrind.HandleAnimaGrind();
                    break;
                case OracleOperationMode.YokaiWatchGrind:
                    await YokaiWatchGrind.HandleYokaiWatchGrind();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Poi.Current.Type == PoiType.Fate || OracleFateManager.CurrentFateId != 0)
            {
                await FateHandler.HandleFate();
            }

            else if (Poi.Current.Type == PoiType.Wait)
            {
                await WaitHandler.HandleWait();
            }

            // Always return false to not block the tree.
            return false;
        }
    }
}