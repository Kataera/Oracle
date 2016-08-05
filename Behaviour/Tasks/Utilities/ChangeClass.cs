using System;
using System.Linq;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;

using Oracle.Helpers;
using Oracle.Managers;
using Oracle.Settings;

namespace Oracle.Behaviour.Tasks.Utilities
{
    public class ChangeClass
    {
        public static async Task<ChangeClassResult> Main(ClassJobType job)
        {
            if (!OracleClassManager.IsCombatClassJob(job))
            {
                Logger.SendErrorLog("Attempted to swap to a non-combat class.");
                return ChangeClassResult.NonCombatClass;
            }

            if (!OracleClassManager.IsClassJobEnabled(job))
            {
                Logger.SendErrorLog("Attempted to swap to a disabled class.");
                return ChangeClassResult.ClassNotEnabled;
            }

            if (!ClassSettings.Instance.ClassGearsets.ContainsValue(job))
            {
                Logger.SendErrorLog("Attempted to swap to a class with no assigned gearset.");
                return ChangeClassResult.NoGearset;
            }

            var previousJob = Core.Player.CurrentJob;
            var gearSet = ClassSettings.Instance.ClassGearsets.FirstOrDefault(kvp => kvp.Value.Equals(job)).Key;
            Logger.SendLog("Changing class from " + OracleClassManager.GetClassJobName(previousJob) + " to " + OracleClassManager.GetClassJobName(job) + ".");
            ChatManager.SendChat("/gs change " + gearSet);
            await Coroutine.Wait(TimeSpan.FromSeconds(7), () => Core.Player.CurrentJob == job);

            if (Core.Player.CurrentJob == previousJob)
            {
                Logger.SendDebugLog("Class did not change from " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob)
                                    + ", likely caused by the game refusing to allow a gearset change.");
                return ChangeClassResult.Failed;
            }

            if (Core.Player.CurrentJob != job)
            {
                Logger.SendErrorLog("Class change failed, current class or job is " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob)
                                    + " when we were expecting " + OracleClassManager.GetClassJobName(job) + ".");
                return ChangeClassResult.WrongClass;
            }

            Logger.SendLog("Successfully changed to " + OracleClassManager.GetClassJobName(job) + ".");
            return ChangeClassResult.Succeeded;
        }
    }

    public enum ChangeClassResult
    {
        Succeeded,

        Failed,

        NonCombatClass,

        NoGearset,

        ClassNotEnabled,

        WrongClass
    }
}