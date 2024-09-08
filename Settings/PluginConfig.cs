using BepInEx.Configuration;
using JoksterCube.AutoFuelLights.Common;
using ServerSync;
using System.Collections.Generic;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Settings
{
    public static class PluginConfig
    {
        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        public static ConfigEntry<Toggle> IsOn = null!;

        public static ConfigEntry<KeyboardShortcut> ToggleShortcut;
        public static ConfigEntry<KeyboardShortcut> FuelShortcut;

        public static ConfigEntry<Toggle> UseWood = null!;
        public static ConfigEntry<Toggle> UseResin = null!;
        public static ConfigEntry<Toggle> UseCoal = null!;
        public static ConfigEntry<Toggle> UseGreydwarfEye = null!;
        public static ConfigEntry<Toggle> UseGuck = null!;

        public static ConfigEntry<Toggle> RefuelFireplaces = null!;
        public static ConfigEntry<Toggle> RefuelHotTubs = null!;
        public static ConfigEntry<Toggle> RefuelLights = null!;

        public static ConfigEntry<float> FireplaceRange;
        public static ConfigEntry<float> DropRange;
        public static ConfigEntry<float> ContainerRange;

        internal static void Build(ConfigFile config, ConfigSync configSync)
        {
            ConfigOptions.Initialize(config, configSync);

            _serverConfigLocked = ConfigOptions.Config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
            _ = configSync.AddLockingConfigEntry(_serverConfigLocked);

            IsOn = ConfigOptions.Config("1 - General", "Is On", Toggle.On, "Plugin is currently on or off.");

            ToggleShortcut = ConfigOptions.Config("2 - Settings", "Toggle Keyboard Shortcut", new KeyboardShortcut(KeyCode.L, KeyCode.RightControl), "Keyboard shortcut to toggle behaviour.");
            FuelShortcut = ConfigOptions.Config("2 - Settings", "Fuel Keyboard Shortcut", new KeyboardShortcut(KeyCode.L), "Keyboard shortcut to toggle fuel from inventory.");

            UseWood = ConfigOptions.Config("3 - Fuel", "Use Wood", Toggle.On, "Should the Wood be used for fuel?");
            UseResin = ConfigOptions.Config("3 - Fuel", "Use Resin", Toggle.On, "Should the Resin be used for fuel?");
            UseCoal = ConfigOptions.Config("3 - Fuel", "Use Coal", Toggle.On, "Should the Coal be used for fuel?");
            UseGreydwarfEye = ConfigOptions.Config("3 - Fuel", "Use Greydwarf Eyes", Toggle.On, "Should the Greydwarf Eyes be used for fuel?");
            UseGuck = ConfigOptions.Config("3 - Fuel", "Use Guck", Toggle.Off, "Should the Guck be used for fuel?");


            FireplaceRange = ConfigOptions.Config("4 - Ranges", "Fireplace Range", 15f,
                             new ConfigDescription("The maximum range for fireplace to be fuelled",
                             new AcceptableValueRange<float>(1f, 50f)));

            DropRange = ConfigOptions.Config("4 - Ranges", "Drop Range", 15f,
                             new ConfigDescription("The maximum range to pull dropped fuel",
                             new AcceptableValueRange<float>(1f, 50f)));

            ContainerRange = ConfigOptions.Config("4 - Ranges", "Fireplace Range", 15f,
                             new ConfigDescription("The maximum range to pull fuel from containers for fireplaces",
                             new AcceptableValueRange<float>(1f, 50f)));
        }

        internal static bool IsAllowedFuel(ItemDrop itemData)
        {
            if (itemData.m_itemData.m_shared.m_name == "$item_wood") return UseWood.IsOn();
            if (itemData.m_itemData.m_shared.m_name == "$item_resin") return UseResin.IsOn();
            if (itemData.m_itemData.m_shared.m_name == "$item_coal") return UseCoal.IsOn();
            if (itemData.m_itemData.m_shared.m_name == "$item_greydwarfeye") return UseGreydwarfEye.IsOn();
            if (itemData.m_itemData.m_shared.m_name == "$item_guck") return UseGuck.IsOn();
            return false;
        }
    }
}
