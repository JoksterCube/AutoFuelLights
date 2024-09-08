using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Domain
{
    internal static class PluginManager
    {
        internal static List<Fireplace> FireplaceFuellingList = new();

        internal static void UpdateFireplaceFuellingList()
        {
            if (!PluginConfig.IsOn.IsOn()) return;

            FireplaceFuellingList.Clear();
            List<Fireplace> ignoreFuelList = new();

            var position = Player.m_localPlayer.transform.position;
            foreach (var collider in Physics.OverlapSphere(position, PluginConfig.FireplaceRange.Value))
            {
                if (collider.transform.parent == null) continue;

                var parent = collider.transform.parent;
                var fireplace = parent.GetComponent<Fireplace>();

                if (fireplace == null) continue;

                if (FireplaceFuellingList.Contains(fireplace) || ignoreFuelList.Contains(fireplace)) continue;

                if (!PluginConfig.IsAllowedFuel(fireplace.m_fuelItem))
                {
                    Plugin.AutoFuelLightsLogger.LogInfo(fireplace.name + ", " + fireplace.m_fuelItem.m_itemData.m_shared.m_name);
                    ignoreFuelList.Add(fireplace);
                    continue;
                }

                FireplaceFuellingList.Add(fireplace);
            }

            PlayerExtensions.FormatedTopLeftMessage(Constants.FireplacesSelectedMessage, FireplaceFuellingList.Count);
            PlayerExtensions.FormatedTopLeftMessage(Constants.FireplacesIgnoredMessage, ignoreFuelList.Count);
        }
    }
}
