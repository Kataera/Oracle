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

            Logger.SendLog("Changing class from " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + " to "
                           + OracleClassManager.GetClassJobName(job) + ".");

            var gearSet = ClassSettings.Instance.ClassGearsets.FirstOrDefault(kvp => kvp.Value.Equals(job)).Key;
            ChatManager.SendChat("/gs change " + gearSet);
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => Core.Player.CurrentJob == job);

            // Check whether or not the class change succeeded.
            if (Core.Player.CurrentJob != job)
            {
                Logger.SendErrorLog("Class change failed, we've changed to " + OracleClassManager.GetClassJobName(Core.Player.CurrentJob) + " when we expected "
                                    + OracleClassManager.GetClassJobName(job) + ".");
                return ChangeClassResult.WrongClass;
            }

            Logger.SendLog("Successfully changed to " + OracleClassManager.GetClassJobName(job) + ".");
            return ChangeClassResult.Success;
        }
    }

    public enum ChangeClassResult
    {
        Success,

        NonCombatClass,

        NoGearset,

        ClassNotEnabled,

        WrongClass
    }
}