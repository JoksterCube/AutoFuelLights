using JoksterCube.AutoFuelLights.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoksterCube.AutoFuelLights.Domain
{
    internal static class FuelManager
    {
        private static List<Container> Containers = new();
        internal static async void Refuel(ZNetView zview, int refuelCount)
        {
            foreach (var fuelSources in PluginConfig.PrioritizedFuelSources)
            {
                var fueled = fuelSources switch
                {
                    FuelSource.Dropped => await RefuelFromDrops(zview, refuelCount),
                    FuelSource.Inventory => await RefuelFromInventory(zview, refuelCount),
                    FuelSource.Containers => await RefuelFromContainers(zview, refuelCount),
                    _ => false
                };
                if (fueled) return;
            }
        }

        private static async Task<bool> RefuelFromDrops(ZNetView zview, int refuelCount)
        {
            throw new NotImplementedException();
        }

        private static async Task<bool> RefuelFromInventory(ZNetView zview, int refuelCount)
        {
            throw new NotImplementedException();
        }

        private static async Task<bool> RefuelFromContainers(ZNetView zview, int refuelCount)
        {
            throw new NotImplementedException();
        }

        internal static void AddContainer(Container container, ZNetView zview)
        {
            try
            {
                if (container.GetInventory() == null || zview?.GetZDO() == null ||
                    (!container.name.StartsWith("piece_", StringComparison.Ordinal) &&
                     !container.name.StartsWith("Container", StringComparison.Ordinal) &&
                     zview.GetZDO().GetLong("creator".GetStableHashCode()) == 0)) return;
                Containers.Add(container);
            }
            catch { }
        }

        internal static void RemoveContainer(Container container)
        {

        }
    }
}
