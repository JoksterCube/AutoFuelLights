using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JoksterCube.AutoFuelLights.Domain;
using JoksterCube.AutoFuelLights.Settings;
using ServerSync;

namespace JoksterCube.AutoFuelLights
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        internal const string ModName = "AutoFuelLights";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "JoksterCube";
        private const string ModGUID = $"{Author}.{ModName}";

        private static string ConfigFileName = $"{ModGUID}.cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;

        internal static string ConnectionError = "";

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource AutoFuelLightsLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
        {
            DisplayName = ModName,
            CurrentVersion = ModVersion,
            MinimumRequiredVersion = ModVersion
        };

        public void Awake()
        {
            PluginConfig.Build(Config, ConfigSync);

            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        public void Update() => InputManager.Update(this);

        private void OnDestroy() => Config.Save();

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                AutoFuelLightsLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                AutoFuelLightsLogger.LogError($"There was an issue loading your {ConfigFileName}");
                AutoFuelLightsLogger.LogError("Please check your config entries for spelling and format!");
            }
        }

    }
}