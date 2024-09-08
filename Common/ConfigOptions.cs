using BepInEx.Configuration;
using BepInEx;
using JetBrains.Annotations;
using ServerSync;
using System;

namespace JoksterCube.AutoFuelLights.Common
{
    internal static class ConfigOptions
    {
        private static ConfigFile ConfigFile;
        private static ConfigSync ConfigSync;

        internal static void Initialize(ConfigFile config, ConfigSync configSync)
        {
            ConfigFile = config;
            ConfigSync = configSync;
        }

        internal static ConfigEntry<T> Config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription = new(description.Description + (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"), description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = ConfigFile.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        internal static ConfigEntry<T> Config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) =>
            Config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        private class ConfigurationManagerAttributes
        {
            [UsedImplicitly] public int? Order = null!;
            [UsedImplicitly] public bool? Browsable = null!;
            [UsedImplicitly] public string Category = null!;
            [UsedImplicitly] public Action<ConfigEntryBase> CustomDrawer = null!;
        }

        class AcceptableShortcuts : AcceptableValueBase
        {
            public AcceptableShortcuts() : base(typeof(KeyboardShortcut))
            {
            }

            public override object Clamp(object value) => value;
            public override bool IsValid(object value) => true;

            public override string ToDescriptionString() => $"# Acceptable values: {string.Join(", ", UnityInput.Current.SupportedKeyCodes)}";
        }
    }
}
