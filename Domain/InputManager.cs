using JoksterCube.AutoFuelLights.Common;
using JoksterCube.AutoFuelLights.Settings;

namespace JoksterCube.AutoFuelLights.Domain
{
    internal static class InputManager
    {
        internal static void Update(Plugin plugin)
        {
            if (PluginConfig.ToggleShortcut.Value.IsKeyDown())
            {
                PluginConfig.IsOn.Value = PluginConfig.IsOn.Value.Not();

                plugin.Config.Save();

                PlayerExtensions.FormatedCenterMessage(Constants.PluginToggleMessage, PluginConfig.IsOn.Value.ToString());
            }

            if (PluginConfig.FuelShortcut.Value.IsKeyDown())
            {
                PluginManager.UpdateFireplaceFuellingList();
            }
        }
    }
}
