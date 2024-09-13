using JoksterCube.AutoFuelLights.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace JoksterCube.AutoFuelLights.Domain
{
	internal static class ContainerManager
	{
		private static List<Container> LocalContainers = new();

		internal static void AddContainer(Container container, ZNetView zview)
		{
			try
			{
				if (container.GetInventory() == null || zview?.GetZDO() == null ||
					(!container.name.StartsWith("piece_", StringComparison.Ordinal) &&
					 !container.name.StartsWith("Container", StringComparison.Ordinal) &&
					 zview.GetZDO().GetLong("creator".GetStableHashCode()) == 0)) return;
				LocalContainers.Add(container);
			}
			catch { }
		}

		internal static void RemoveContainer(Container container) => LocalContainers.Remove(container);

		internal static IEnumerable<Container> GetAvailableContainersWithItems(string itemName) => LocalContainers
			.Where(x =>
				!x.IsInUse() &&
				Vector3.Distance(x.transform.position, Player.m_localPlayer.transform.position) <= PluginConfig.ContainerRange.Value &&
				x.GetInventory().HaveItem(itemName));

		internal static int RemoveItemsFromContainers(string itemName, int ammount)
		{
			foreach (var container in GetAvailableContainersWithItems(itemName))
			{
				container.GetInventory().RemoveItem(itemName, ammount);
			}
		}
	}
}