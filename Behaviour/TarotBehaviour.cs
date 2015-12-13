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

using System.Linq;
using System.Threading.Tasks;

using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;

using Tarot.Behaviour.Tasks;
using Tarot.Behaviour.Tasks.Utilities;
using Tarot.Enumerations;
using Tarot.Helpers;
using Tarot.Managers;

using TreeSharp;

namespace Tarot.Behaviour
{
    internal static class TarotBehaviour
    {
        public static Composite Behaviour
        {
            get { return CreateBehaviour(); }
        }

        public static void ClearPoi(string reason)
        {
            Logger.SendLog(reason);
            Poi.Clear(reason);
        }

        private static void ClearPoi(string reason, bool sendLog)
        {
            if (sendLog)
            {
                Logger.SendLog(reason);
            }

            Poi.Clear(reason);
        }

        private static Composite CreateBehaviour()
        {
            return new ActionRunCoroutine(coroutine => Main());
        }

        private static async Task<bool> HandleCombat()
        {
            if (TarotFateManager.CurrentFate != null && !TarotFateManager.CurrentFate.IsValid)
            {
                if (TarotFateManager.FateDatabase.GetFateWithId(TarotFateManager.CurrentFate.Id).Type != FateType.Collect)
                {
                    TarotFateManager.ClearCurrentFate("Current FATE is finished.");
                    return true;
                }
            }

            if (Poi.Current.BattleCharacter != null && Poi.Current.BattleCharacter.IsValid && !Poi.Current.BattleCharacter.IsFate
                && !GameObjectManager.Attackers.Contains(Poi.Current.BattleCharacter))
            {
                ClearPoi("Current Poi is not a FATE mob, nor attacking us.");
                return true;
            }

            if (Poi.Current.BattleCharacter != null && Poi.Current.BattleCharacter.IsValid && Poi.Current.BattleCharacter.IsFate)
            {
                var fate = FateManager.GetFateById(Poi.Current.BattleCharacter.FateId);

                if (fate == null)
                {
                    return false;
                }

                if (!LevelSync.IsLevelSyncNeeded(fate))
                {
                    return true;
                }

                if (fate.IsValid)
                {
                    if (fate.Within2D(Core.Player.Location))
                    {
                        await LevelSync.Main(fate);
                    }
                    else
                    {
                        await MoveToFate.Main(true);
                        await LevelSync.Main(fate);
                    }
                }
            }

            return true;
        }

        private static async Task<bool> HandleFate()
        {
            if (Core.Player.Distance(TarotFateManager.CurrentFate.Location) > TarotFateManager.CurrentFate.Radius)
            {
                await MoveToFate.Main(false);
            }

            if (TarotFateManager.CurrentFate == null)
            {
                return false;
            }

            if (GameObjectManager.Attackers.Any(mob => !mob.IsFateGone) && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            if (LevelSync.IsLevelSyncNeeded(TarotFateManager.CurrentFate))
            {
                await LevelSync.Main(TarotFateManager.CurrentFate);
            }

            return await FateRunner.Main();
        }

        private static async Task<bool> HandleWait()
        {
            if (GameObjectManager.Attackers.Any(mob => !mob.IsFateGone) && !Core.Player.IsMounted)
            {
                ClearPoi("We're being attacked.", false);
                return true;
            }

            if (await TarotFateManager.AnyViableFates())
            {
                ClearPoi("Viable FATE detected.");
                return true;
            }

            return await WaitRunner.Main();
        }

        private static async Task<bool> Main()
        {
            if (TarotFateManager.FateDatabase == null)
            {
                await BuildFateDatabase.Main();
            }

            if (Poi.Current == null)
            {
                return false;
            }

            switch (Poi.Current.Type)
            {
                case PoiType.Kill:
                    await HandleCombat();
                    return false;
                case PoiType.Fate:
                    await HandleFate();
                    return false;
                case PoiType.Wait:
                    await HandleWait();
                    return false;
            }

            // Always return false to not block the tree.
            return false;
        }
    }
}