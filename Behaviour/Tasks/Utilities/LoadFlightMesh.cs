/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Managers;
using ff14bot.Settings;

using Newtonsoft.Json;

using Oracle.Data;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LoadFlightMesh
    {
        public static async Task<bool> Main()
        {
            Logger.SendLog("Loading flight mesh for the current zone.");

            OracleManager.ZoneFlightMesh = await LoadMesh(WorldManager.ZoneId);
            return true;
        }

        private static async Task<OracleFlightMesh> LoadMesh(ushort zoneId)
        {
            var path = GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Meshes\zone_" + zoneId + ".gz";

            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        using (var sr = new StreamReader(gzip))
                        {
                            Logger.SendLog("Unpacking the mesh file.");
                            var json = await Coroutine.ExternalTask(sr.ReadToEndAsync());

                            Logger.SendLog("Deserialising the mesh file.");
                            var flightMesh =
                                await
                                    Coroutine.ExternalTask(Task.Factory.StartNew(() => JsonConvert.DeserializeObject<OracleFlightMesh>(json)));

                            Logger.SendLog("Flight mesh loaded successfully!");
                            return flightMesh;
                        }
                    }
                }
            }
            Logger.SendErrorLog("Could not find a mesh file for zone " + zoneId + ".");

            return null;
        }
    }
}