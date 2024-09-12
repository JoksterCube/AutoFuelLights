using HarmonyLib;
using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Settings;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Patches
{
    internal class SmelterPatches
    {
        [HarmonyPatch(typeof(Smelter), nameof(Smelter.UpdateSmelter))]
        private static class Smelter_FixedUpdate_Patch
        {
            private static void Postfix(Smelter __instance, ZNetView ___m_nview)
            {
                if (!PluginConfig.IsOn.IsOn() || !Player.m_localPlayer || !___m_nview.IsOwner()) return;

                if (!PluginConfig.IsAllowedSmelterType(__instance)) return;

                var distance = Vector3.Distance(Player.m_localPlayer.transform.position, __instance.transform.position);
                if (distance > PluginConfig.FireplaceRange.Value) return;

                if (!PluginConfig.IsAllowedFuel(__instance.m_fuelItem)) return;
            }
        }
    }
}
