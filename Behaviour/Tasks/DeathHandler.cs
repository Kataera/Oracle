using System;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Behavior;

namespace Oracle.Behaviour.Tasks
{
    internal static class DeathHandler
    {
        public static async Task<bool> HandleDeath()
        {
            await Coroutine.Wait(TimeSpan.FromSeconds(5), () => CommonBehaviors.IsLoading);
            await CommonTasks.HandleLoading();
            await Coroutine.Sleep(TimeSpan.FromSeconds(2));

            return true;
        }
    }
}