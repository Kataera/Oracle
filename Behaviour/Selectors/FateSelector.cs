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

namespace Tarot.Behaviour.Selectors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Clio.Utilities;

    using ff14bot;
    using ff14bot.Enums;
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Data.FateTypes;
    using global::Tarot.Enumerations;
    using global::Tarot.Helpers;

    using TreeSharp;

    using Action = TreeSharp.Action;

    // TODO: Refactor this to use a static class rather than Singleton
    internal sealed class FateSelector
    {
        private static readonly object SyncRootObject = new object();

        private static volatile FateSelector instance;

        private bool waitFlag;

        private FateSelector() {}

        public static FateSelector Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new FateSelector();
                            instance.CreateBehaviour();
                            instance.waitFlag = false;
                        }
                    }
                }

                return instance;
            }
        }

        public Composite Behaviour { get; private set; }

        private static async Task<bool> CheckCurrentFateValidity()
        {
            if (MainBehaviour.Instance.CurrentRbFate == null || MainBehaviour.Instance.CurrentFate == null)
            {
                return false;
            }

            if (MainBehaviour.Instance.CurrentRbFate.Status == FateStatus.COMPLETE)
            {
                Logger.SendLog("Current FATE is complete, selecting a new one.");
                MainBehaviour.Instance.SetCurrentFate(null, null);
                Poi.Clear("Current FATE is complete.");

                return false;
            }

            if (!MainBehaviour.Instance.CurrentRbFate.IsValid)
            {
                Logger.SendLog("Current FATE is no longer valid, selecting a new one.");
                MainBehaviour.Instance.SetCurrentFate(null, null);
                Poi.Clear("Current FATE is no longer valid.");

                return false;
            }

            return true;
        }

        private void SelectFate()
        {
            /*
                TODO:
                    Implement other selection strategies.
                    Implement fate blacklist.
            */
            var fateList = FateManager.ActiveFates.ToList();
            FateData rebornFateData = null;
            Fate tarotFate = null;

            foreach (var fate in fateList)
            {
                var fateId = Convert.ToInt32(fate.Id);

                // Check if supported.
                // TODO: Rewrite this so we're not using return.
                if (MainBehaviour.Instance.FateDatabase.GetFateWithId(fateId).SupportLevel
                    >= (int) FateSupportLevel.Unsupported)
                {
                    return;
                }

                //// TODO: Check if blacklisted.

                if (rebornFateData == null)
                {
                    rebornFateData = fate;
                    tarotFate = MainBehaviour.Instance.FateDatabase.GetFateWithId(fateId);
                }

                if (fate.Location.Distance2D(Core.Player.Location)
                    < rebornFateData.Location.Distance2D(Core.Player.Location))
                {
                    rebornFateData = fate;
                    tarotFate = MainBehaviour.Instance.FateDatabase.GetFateWithId(fateId);
                }
            }

            if (rebornFateData != null)
            {
                // Reformat fateType string if it's a Mega-Boss fate.
                var fateType = tarotFate.Type == (int) FateType.MegaBoss
                                   ? "Mega-Boss"
                                   : Enum.GetName(typeof(FateType), tarotFate.Type);
                Logger.SendLog("Selected FATE: " + rebornFateData.Name + " (" + fateType + ").");

                MainBehaviour.Instance.SetCurrentFate(rebornFateData, tarotFate);

                this.waitFlag = false;
            }
            else if (!this.waitFlag)
            {
                Logger.SendLog("There's no available FATEs, moving to wait location.");
                Poi.Current = new Poi(new Vector3(-9.798996f, 46.99999f, -14.16332f), PoiType.Wait);

                this.waitFlag = true;
            }
        }

        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                new ActionRunCoroutine(coroutine => CheckCurrentFateValidity()),
                new Action(action => this.SelectFate())
            };
            this.Behaviour = new PrioritySelector(behaviours);
        }
    }
}