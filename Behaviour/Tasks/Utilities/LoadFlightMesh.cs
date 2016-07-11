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
        public static FileStream MeshFileStream { get; set; }

        public static async Task<bool> Main()
        {
            Logger.SendLog("Loading flight mesh for the current zone.");

            OracleMovementManager.ZoneFlightMesh = await LoadMesh(WorldManager.ZoneId);
            return true;
        }

        private static async Task<OracleFlightMesh> LoadMesh(ushort zoneId)
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