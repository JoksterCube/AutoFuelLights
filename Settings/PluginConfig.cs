using BepInEx.Configuration;
using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Domain;
using ServerSync;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Settings
{
    public static class PluginConfig
    {
        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        public static ConfigEntry<Toggle> IsOn = null!;

        public static ConfigEntry<Toggle> DropFuel = null!;
        public static ConfigEntry<Toggle> InventoryFuel = null!;
        public static ConfigEntry<Toggle> ContainerFuel = null!;

        public static ConfigEntry<Toggle> OneAtTheTime = null!;

        public static ConfigEntry<KeyboardShortcut> ToggleShortcut;
        public static ConfigEntry<KeyboardShortcut> FuelShortcut;

        public static ConfigEntry<Toggle> UseWood = null!;
        public static ConfigEntry<Toggle> UseResin = null!;
        public static ConfigEntry<Toggle> UseCoal = null!;
        public static ConfigEntry<Toggle> UseGreydwarfEye = null!;
        public static ConfigEntry<Toggle> UseGuck = null!;

        public static ConfigEntry<int> DropFuelPriority;
        public static ConfigEntry<int> InventoryFuelPriority;
        public static ConfigEntry<int> ContainerFuelPriority;

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

            ToggleShortcut = ConfigOptions.Config("2 - Shortcuts", "Toggle Keyboard Shortcut", new KeyboardShortcut(KeyCode.L, KeyCode.RightControl), "Keyboard shortcut to toggle behaviour.");
            FuelShortcut = ConfigOptions.Config("2 - Shortcuts", "Fuel Keyboard Shortcut", new KeyboardShortcut(KeyCode.L), "Keyboard shortcut to toggle fuel from inventory.");

            DropFuel = ConfigOptions.Config("3 - Settings", "Drop Fuel", Toggle.On, "Use dropped fuel or not.");
            InventoryFuel = ConfigOptions.Config("3 - Settings", "Inventory Fuel", Toggle.Off, "Use fuel from inventory or not.");
            ContainerFuel = ConfigOptions.Config("3 - Settings", "Container Fuel", Toggle.On, "Use fuel from containers or not.");

            OneAtTheTime = ConfigOptions.Config("3 - Settings", "One at the time", Toggle.On, "Fuel one at the time or allow multiple fuel at the same time?");

            DropFuelPriority = ConfigOptions.Config("4 - Priority", "Drop Priority", 1, "Dropped fuel priority. (If two values are the same, defaults are used: Drop > Inventory > Containers)");
            InventoryFuelPriority = ConfigOptions.Config("4 - Priority", "Inventory Priority", 2, "Inventory fuel priority. (If two values are the same, defaults are used: Drop > Inventory > Containers)");
            ContainerFuelPriority = ConfigOptions.Config("4 - Priority", "Container Priority", 3, "Container fuel priority. (If two values are the same, defaults are used: Drop > Inventory > Containers)");

            UseWood = ConfigOptions.Config("5 - Fuel", "Use Wood", Toggle.On, "Should the Wood be used for fuel?");
            UseResin = ConfigOptions.Config("5 - Fuel", "Use Resin", Toggle.On, "Should the Resin be used for fuel?");
            UseCoal = ConfigOptions.Config("5 - Fuel", "Use Coal", Toggle.On, "Should the Coal be used for fuel?");
            UseGreydwarfEye = ConfigOptions.Config("5 - Fuel", "Use Greydwarf Eyes", Toggle.On, "Should the Greydwarf Eyes be used for fuel?");
            UseGuck = ConfigOptions.Config("5 - Fuel", "Use Guck", Toggle.Off, "Should the Guck be used for fuel?");


            FireplaceRange = ConfigOptions.Config("6 - Ranges", "Fireplace Range", 15f,
                             new ConfigDescription("The maximum range for fireplace to be fuelled",
                             new AcceptableValueRange<float>(1f, 50f)));

            DropRange = ConfigOptions.Config("6 - Ranges", "Drop Range", 15f,
                             new ConfigDescription("The maximum range to pull dropped fuel",
                             new AcceptableValueRange<float>(1f, 50f)));

            ContainerRange = ConfigOptions.Config("6 - Ranges", "Fireplace Range", 15f,
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

        internal static IEnumerable<FuelSource> PrioritizedFuelSources =>
            FuelPrioirityDictionary.OrderBy(x => x.Value).Select(x => x.Key);

        internal static bool IsAllowedFireplaceType(Fireplace fireplace)
        {
            return true;
        }

        internal static bool IsAllowedSmelterType(Smelter smelter)
        {
            return true;
        }

        private static Dictionary<FuelSource, int> FuelPrioirityDictionary
        {
            get
            {
                Dictionary<FuelSource, int> fuelPrioirityDictionary = new();
                if (DropFuel.IsOn()) fuelPrioirityDictionary.Add(FuelSource.Dropped, DropFuelPriority.Value);
                if (InventoryFuel.IsOn()) fuelPrioirityDictionary.Add(FuelSource.Inventory, InventoryFuelPriority.Value);
                if (ContainerFuel.IsOn()) fuelPrioirityDictionary.Add(FuelSource.Containers, ContainerFuelPriority.Value);
                return fuelPrioirityDictionary;
            }
        }
    }
}