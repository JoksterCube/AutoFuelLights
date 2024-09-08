using BepInEx.Configuration;
using System.Linq;
using UnityEngine;

namespace JoksterCube.AutoFuelLights.Common
{
    internal static class KeyboardExtensions
    {
        public static bool IsKeyDown(this KeyboardShortcut shortcut) =>
            shortcut.MainKey != KeyCode.None && Input.GetKeyDown(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);

        public static bool IsKeyHeld(this KeyboardShortcut shortcut) =>
            shortcut.MainKey != KeyCode.None && Input.GetKey(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
    }
}
