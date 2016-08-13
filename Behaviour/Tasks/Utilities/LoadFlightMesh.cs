using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using Buddy.Coroutines;

using ff14bot.Managers;
using ff14bot.Settings;

using Newtonsoft.Json;

using Oracle.Data.Meshes;
using Oracle.Helpers;
using Oracle.Managers;

namespace Oracle.Behaviour.Tasks.Utilities
{
    internal static class LoadFlightMesh
    {
        public static FileStream MeshFileStream { get; set; }

        private static async Task<FlightMesh> LoadMesh(ushort zoneId)
        {
            var path = GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Meshes\zone_" + zoneId + ".gz";

            if (File.Exists(path))
            {
                using (MeshFileStream = new FileStream(path, FileMode.Open))
                {
                    using (var gzip = new GZipStream(MeshFileStream, CompressionMode.Decompress))
                    {
                        using (var sr = new StreamReader(gzip))
                        {
                            Logger.SendDebugLog("Unpacking the mesh file.");
                            var json = await Coroutine.ExternalTask(sr.ReadToEndAsync());

                            Logger.SendDebugLog("Deserialising the mesh file.");
                            var flightMesh = await Coroutine.ExternalTask(Task.Factory.StartNew(() => JsonConvert.DeserializeObject<FlightMesh>(json)));

                            if (flightMesh.Graph == null)
                            {
                                Logger.SendErrorLog("Flight mesh loading failed.");
                                return null;
                            }

                            Logger.SendLog("Flight mesh loaded successfully!");
                            return flightMesh;
                        }
                    }
                }
            }

            Logger.SendErrorLog("Could not find a mesh file for zone " + zoneId + ".");
            return null;
        }

        public static async Task<bool> Main()
        {
            OracleMovementManager.ZoneFlightMesh = await LoadMesh(WorldManager.ZoneId);
            return true;
        }
    }
}