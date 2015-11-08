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

    using TreeSharp;

    internal static class MainBehaviour
    {
        public static Composite Behaviour
        {
            get
            {
                return CreateBehaviour();
            }
        }

        public static Fate CurrentFate { get; private set; }

        public static FateData CurrentRbFate { get; private set; }

        public static FateDatabase FateDatabase { get; set; }

        public static void SetCurrentFate(FateData rebornFateData, Fate tarotFateData)
        {
            CurrentFate = tarotFateData;
            CurrentRbFate = rebornFateData;

            if (rebornFateData != null)
            {
                Poi.Current = new Poi(rebornFateData, PoiType.Fate);
                Tarot.CurrentPoi = Poi.Current;
            }
        }

        private static Composite CreateBehaviour()
        {
            Composite[] behaviours = { FateSelector.Behaviour, NavigationHandler.Behaviour, FateHandler.Behaviour };

            return new Sequence(behaviours);
        }
    }
}