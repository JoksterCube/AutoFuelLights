using HarmonyLib;
using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Domain;
using JoksterCube.AutoFuelLights.Settings;
using System.Threading.Tasks;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Patches
{
    internal class FireplacePatches
    {
        [HarmonyPatch(typeof(Fireplace), nameof(Fireplace.UpdateFireplace))]
        private static class Fireplace_UpdateFireplace_Patch
        {
            private static void Postfix(Fireplace __instance, ZNetView ___m_nview)
            {
                if (!PluginConfig.IsOn.IsOn() || !Player.m_localPlayer || !___m_nview.IsOwner()) return;

                if (!PluginConfig.IsAllowedFireplaceType(__instance)) return;

                var distance = Vector3.Distance(Player.m_localPlayer.transform.position, __instance.transform.position);
                if (distance > PluginConfig.FireplaceRange.Value) return;

                if (!PluginConfig.IsAllowedFuel(__instance.m_fuelItem)) return;

                Refuel(__instance, ___m_nview);
            }

            private static async void Refuel(Fireplace fireplace, ZNetView zview)
            {
                try
                {
                    await Task.Delay(100);

                    var currentFuel = zview.GetZDO().GetFloat(ZDOVars.s_fuel);

                    var refuelCount = (int)(fireplace.m_maxFuel - Mathf.Ceil(currentFuel));

                    if (refuelCount <= 0) return;

                    FuelManager.Refuel(zview, fireplace.m_fuelItem, refuelCount);

                    //zview.InvokeRPC(nameof(fireplace.RPC_AddFuelAmount), 1f);
                }
                catch { }
            }
        }
    }
}
