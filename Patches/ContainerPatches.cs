using HarmonyLib;
using JoksterCube.AutoFuelLights.Domain;

namespace JoksterCube.AutoFuelLights.Patches
{
    internal class ContainerPatches
    {
        [HarmonyPatch(typeof(Container), nameof(Container.Awake))]
        static class ContainerAwakePatch
        {
            static void Postfix(Container __instance, ZNetView ___m_nview)
            {
                Plugin.AutoFuelLightsLogger.LogInfo($"{__instance.m_name}");

                //ContainerManager.AddContainer(__instance, ___m_nview);
            }
        }

        [HarmonyPatch(typeof(Container), nameof(Container.OnDestroyed))]
        static class ContainerOnDestroyedPatch
        {
            static void Prefix(Container __instance)
            {
                //ContainerManager.RemoveContainer(__instance);
            }
        }
    }
}
