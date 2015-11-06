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

namespace Tarot.Behaviour
{
    using ff14bot.Helpers;
    using ff14bot.Managers;

    using global::Tarot.Behaviour.Handlers;
    using global::Tarot.Behaviour.Selectors;
    using global::Tarot.Data;
    using global::Tarot.Data.FateTypes;
    using global::Tarot.Helpers;

    using TreeSharp;

    internal sealed class MainBehaviour
    {
        private static readonly object SyncRootObject = new object();

        private static volatile MainBehaviour instance;

        private MainBehaviour(FateDatabase database)
        {
            this.FateDatabase = database;
        }

        public static MainBehaviour Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new MainBehaviour(XmlParser.GetFateDatabase(true));
                            instance.CreateBehaviour();
                        }
                    }
                }

                return instance;
            }
        }

        public Composite Behaviour { get; private set; }

        public Fate CurrentFate { get; private set; }

        public FateData CurrentRbFate { get; private set; }

        public FateDatabase FateDatabase { get; set; }

        public void SetCurrentFate(FateData rebornFateData, Fate tarotFateData)
        {
            this.CurrentFate = tarotFateData;
            this.CurrentRbFate = rebornFateData;

            if (rebornFateData != null)
            {
                Poi.Current = new Poi(rebornFateData, PoiType.Fate);
            }
        }

        private void CreateBehaviour()
        {
            Composite[] behaviours =
            {
                FateSelector.Instance.Behaviour, NavigationSelector.Instance.Behaviour,
                NavigationHandler.Instance.Behaviour, FateHandler.Instance.Behaviour
            };
            this.Behaviour = new Sequence(behaviours);
        }
    }
}