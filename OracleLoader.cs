using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;

using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;

using ICSharpCode.SharpZipLib.Zip;

using TreeSharp;

using Action = TreeSharp.Action;

namespace Oracle
{
    public class LisbethLoader : BotBase
    {
        private const string ProjectName = "Oracle";
        private const string ProjectClass = "Oracle.OracleBot";
        private static readonly object Locker = new object();
        private static readonly string ProjectAssembly = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle\Oracle.dll");
        private static readonly string VersionPath = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle\version.txt");
        private static readonly string BaseDir = Path.Combine(Environment.CurrentDirectory, @"BotBases\Oracle");
        private static readonly string ProjectTypeFolder = Path.Combine(Environment.CurrentDirectory, @"BotBases");
        private static readonly Color LogColor = Color.FromRgb(179, 179, 255);
        private static volatile bool updaterStarted, updaterFinished, loaded;

        public LisbethLoader()
        {
            if (updaterStarted)
            {
                return;
            }

            updaterStarted = true;
            Task.Factory.StartNew(Update);
        }

        public override bool IsAutonomous => true;

        public override string Name => "Oracle";

        public static object Oracle { get; set; }

        private static MethodInfo OracleButton { get; set; }

        private static MethodInfo OracleRoot { get; set; }

        private static MethodInfo OracleStart { get; set; }

        private static MethodInfo OracleStop { get; set; }

        public override PulseFlags PulseFlags => PulseFlags.All;

        public override bool RequiresProfile => false;

        public override Composite Root
        {
            get
            {
                if (!loaded && Oracle == null && updaterFinished)
                {
                    LoadOracle();
                }

                return Oracle != null ? (Composite) OracleRoot.Invoke(Oracle, null) : new Action();
            }
        }

        public override bool WantButton => true;

        private static bool Clean(string directory)
        {
            foreach (var file in new DirectoryInfo(directory).GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    return false;
                }
            }

            foreach (var dir in new DirectoryInfo(directory).GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Extract(byte[] files, string directory)
        {
            using (var stream = new MemoryStream(files))
            {
                var zip = new FastZip();

                try
                {
                    zip.ExtractZip(stream, directory, FastZip.Overwrite.Always, null, null, null, false, true);
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    return false;
                }
            }

            return true;
        }

        private static async Task<byte[]> GetLatestProduct()
        {
            using (var client = new HttpClient())
            {
                Uri uri;
                try
                {
                    uri = new Uri(@"https://siune.azurewebsites.net/api/update");
                }
                catch
                {
                    return null;
                }

                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();

                try
                {
                    var content = new StringContent(ProjectName);
                    var response = await client.PostAsync(uri, content);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    return await response.Content.ReadAsByteArrayAsync();
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    return null;
                }
            }
        }

        private static async Task<string> GetLatestVersion()
        {
            using (var client = new HttpClient())
            {
                Uri uri;
                try
                {
                    uri = new Uri(@"https://siune.azurewebsites.net/");
                }
                catch
                {
                    return null;
                }

                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();

                try
                {
                    var response = await client.GetAsync(@"api/version/" + ProjectName);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    var result = await response.Content.ReadAsStringAsync();
                    return result.Replace("\"", string.Empty);
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                    return null;
                }
            }
        }

        private static string GetLocalVersion()
        {
            if (!File.Exists(VersionPath))
            {
                return null;
            }

            try
            {
                var version = File.ReadAllText(VersionPath);
                return version;
            }
            catch
            {
                return null;
            }
        }

        private static object Load()
        {
            RedirectAssembly();

            var assembly = LoadAssembly(ProjectAssembly);
            if (assembly == null)
            {
                return null;
            }

            Type baseType;
            try
            {
                baseType = assembly.GetType(ProjectClass);
            }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }

            object botBase;
            try
            {
                botBase = Activator.CreateInstance(baseType);
            }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }

            if (botBase != null)
            {
                Log(ProjectName + " was loaded successfully.");
            }
            else
            {
                Log("Could not load " + ProjectName + ". This can be due to a new version of Rebornbuddy being released. An update should be ready soon.");
            }

            return botBase;
        }

        private static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(path);
            }
            catch (Exception e)
            {
                Logging.WriteException(e);
            }

            return assembly;
        }

        private static void LoadOracle()
        {
            lock (Locker)
            {
                if (Oracle != null)
                {
                    return;
                }

                Oracle = Load();
                loaded = true;
                if (Oracle == null)
                {
                    return;
                }

                OracleStart = Oracle.GetType().GetMethod("Start");
                OracleStop = Oracle.GetType().GetMethod("Stop");
                OracleButton = Oracle.GetType().GetMethod("OnButtonPress");
                OracleRoot = Oracle.GetType().GetMethod("GetRoot");
            }
        }

        private static void Log(string message)
        {
            message = "[" + ProjectName + " v" + GetLocalVersion() + "] " + message;
            Logging.Write(LogColor, message);
        }

        public override void OnButtonPress()
        {
            if (!loaded && Oracle == null && updaterFinished)
            {
                LoadOracle();
            }

            if (Oracle != null)
            {
                OracleButton.Invoke(Oracle, null);
            }
        }

        public static void RedirectAssembly()
        {
            ResolveEventHandler handler = (sender, args) =>
            {
                var name = Assembly.GetEntryAssembly().GetName().Name;
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != name ? null : Assembly.GetEntryAssembly();
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }

        public override void Start()
        {
            if (!loaded && Oracle == null && updaterFinished)
            {
                LoadOracle();
            }

            if (Oracle != null)
            {
                OracleStart.Invoke(Oracle, null);
            }
        }

        public override void Stop()
        {
            if (!loaded && Oracle == null && updaterFinished)
            {
                LoadOracle();
            }

            if (Oracle != null)
            {
                OracleStop.Invoke(Oracle, null);
            }
        }

        private static void Update()
        {
            // TODO: Remove when Oracle is on Siune and uncomment the rest.
            updaterFinished = true;
            LoadOracle();
            return;

            /*
            var stopwatch = Stopwatch.StartNew();
            var local = GetLocalVersion();
            var latest = GetLatestVersion().Result;

            if (local == latest || latest == null)
            {
                updaterFinished = true;
                LoadOracle();
                return;
            }

            Log("Updating " + ProjectName + " to version " + latest + ".");
            var bytes = GetLatestProduct().Result;
            if (bytes == null || bytes.Length == 0)
            {
                return;
            }

            if (!Clean(BaseDir))
            {
                Log("Could not clean directory for update.");
                updaterFinished = true;
                return;
            }

            Log("Extracting new files...");
            if (!Extract(bytes, ProjectTypeFolder))
            {
                Log("Could not extract new files.");
                updaterFinished = true;
                return;
            }

            if (File.Exists(VersionPath))
            {
                File.Delete(VersionPath);
            }
            try
            {
                File.WriteAllText(VersionPath, latest);
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }

            stopwatch.Stop();
            Log(ProjectName + " update complete in " + stopwatch.ElapsedMilliseconds + " ms.");
            updaterFinished = true;
            LoadOracle();
            */
        }
    }
}