using HarmonyLib;
using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Settings;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Patches
{
    internal class FireplacePatch
    {
        [HarmonyPatch(typeof(Fireplace), "UpdateFireplace")]
        private static class Fireplace_UpdateFireplace_Patch
        {
            private static void Postfix(Fireplace __instance, ZNetView ___m_nview)
            {
                if (!PluginConfig.IsOn.IsOn() || !Player.m_localPlayer || !___m_nview.IsOwner()) return;

                var distance = Vector3.Distance(Player.m_localPlayer.transform.position, __instance.transform.position);
                if (distance > PluginConfig.FireplaceRange.Value) return;

                if (!PluginConfig.IsAllowedFuel(__instance.m_fuelItem)) return;

                var currentFuel = ___m_nview.GetZDO().GetFloat("fuel");

                Plugin.AutoFuelLightsLogger.LogInfo(__instance.name + " current fuel: " + currentFuel);

            }
        }
    }
}
