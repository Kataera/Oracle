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

namespace Tarot.Data.FateTypes
{
    using global::Tarot.Enumerations;

    /*
        This class is a null implementation of Fate, it has no supporting coroutine
        and it should only be created when a database lookup returns no results.
    */

    internal class NullFate : Fate
    {
        public NullFate()
        {
            this.ChainIdFailure = 0;
            this.ChainIdSuccess = 0;
            this.Id = 0;
            this.ItemId = 0;
            this.Level = 0;
            this.Name = string.Empty;
            this.NpcId = 0;
            this.SupportLevel = FateSupportLevel.Unsupported;
            this.Type = FateType.Null;
        }
    }
}