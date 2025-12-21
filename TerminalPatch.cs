using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace KroesTerminal.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        // FOUND IT
        [HarmonyPatch("OnSubmit")]
        [HarmonyPrefix]
        private static bool OnSubmitPrefix(Terminal __instance)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            string command = text.ToLower();
            KroesTerminal.Log.LogInfo($"OnSubmitPrefix: {command}");
            return true;
        }

        [HarmonyPatch("LoadNewNode")]
        [HarmonyPrefix]
        private static bool LoadNewNodePrefix(ref TerminalNode node)
        {
            KroesTerminal.Log.LogInfo($"(string) displayText: {node.displayText}");
            KroesTerminal.Log.LogInfo($"(string) terminalEvent: {node.terminalEvent}");
            KroesTerminal.Log.LogInfo($"(bool) clearPreviousText: {node.clearPreviousText}");
            KroesTerminal.Log.LogInfo($"(int) maxCharactersToType: {node.maxCharactersToType}");

            KroesTerminal.Log.LogInfo($"(int) buyItemIndex: {node.buyItemIndex}");
            KroesTerminal.Log.LogInfo($"(int) buyVehicleIndex: {node.buyVehicleIndex}");
            KroesTerminal.Log.LogInfo($"(bool) isConfirmationNode: {node.isConfirmationNode}");
            KroesTerminal.Log.LogInfo($"(int) buyRerouteToMoon: {node.buyRerouteToMoon}");
            KroesTerminal.Log.LogInfo($"(int) displayPlanetInfo: {node.displayPlanetInfo}");

            KroesTerminal.Log.LogInfo($"(int) shipUnlockableID: {node.shipUnlockableID}");
            KroesTerminal.Log.LogInfo($"(bool) buyUnlockable: {node.buyUnlockable}");
            KroesTerminal.Log.LogInfo($"(bool) returnFromStorage: {node.returnFromStorage}");
            KroesTerminal.Log.LogInfo($"(int) itemCost: {node.itemCost}");

            KroesTerminal.Log.LogInfo($"(int) creatureFileID: {node.creatureFileID}");
            KroesTerminal.Log.LogInfo($"(string) creatureName: {node.creatureName}");
            KroesTerminal.Log.LogInfo($"(int) storyLogFileID: {node.storyLogFileID}");

            KroesTerminal.Log.LogInfo($"(bool) overrideOptions: {node.overrideOptions}");
            KroesTerminal.Log.LogInfo($"(bool) acceptAnything: {node.acceptAnything}");

            KroesTerminal.Log.LogInfo($"(CompatibleNoun[]) terminalOptions: {(node.terminalOptions == null ? "null" : node.terminalOptions.Length.ToString())}");

            KroesTerminal.Log.LogInfo($"(int) playSyncedClip: {node.playSyncedClip}");

            KroesTerminal.Log.LogInfo($"(bool) loadImageSlowly: {node.loadImageSlowly}");
            KroesTerminal.Log.LogInfo($"(bool) persistentImage: {node.persistentImage}");

            return true;
        }

    }
}