﻿/*
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

namespace Tarot.Behaviour.Coroutines
{
    using System.Threading.Tasks;

    using TreeSharp;

    internal sealed class KillFate
    {
        private static readonly object SyncRootObject = new object();

        private static volatile KillFate instance;

        private KillFate() {}

        public static KillFate Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRootObject)
                    {
                        if (instance == null)
                        {
                            instance = new KillFate();
                            instance.CreateCoroutine();
                        }
                    }
                }

                return instance;
            }
        }

        public ActionRunCoroutine Coroutine { get; private set; }

        private static async Task<bool> KillFateTask()
        {
            // TODO: Write fate task.
            return true;
        }

        private void CreateCoroutine()
        {
            this.Coroutine = new ActionRunCoroutine(coroutine => KillFateTask());
        }
    }
}