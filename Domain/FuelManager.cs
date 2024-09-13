using JoksterCube.AutoFuelLights.Settings;
using System;
using System.Threading.Tasks;

namespace JoksterCube.AutoFuelLights.Domain
{
    internal static class FuelManager
    {
        internal static async void Refuel(ZNetView zview, string fuelItem, int refuelCount)
        {
            foreach (var fuelSources in PluginConfig.PrioritizedFuelSources)
            {
                var fueled = fuelSources switch
                {
                    FuelSource.Dropped => await RefuelFromDrops(zview, fuelItem, refuelCount),
                    FuelSource.Inventory => await RefuelFromInventory(zview, fuelItem, refuelCount),
                    FuelSource.Containers => await RefuelFromContainers(zview, fuelItem, refuelCount),
                    _ => false
                };
                if (fueled) return;
            }
        }

        private static async Task<bool> RefuelFromDrops(ZNetView zview, string fuelItem, int refuelCount)
        {
            throw new NotImplementedException();
        }

        private static async Task<bool> RefuelFromInventory(ZNetView zview, string fuelItem, int refuelCount)
        {
            throw new NotImplementedException();
        }

        private static async Task<bool> RefuelFromContainers(ZNetView zview, string fuelItem, int refuelCount)
        {
            ContainerManager.GetAvailableContainersWithItems(fuelItem)
        }
    }
}
