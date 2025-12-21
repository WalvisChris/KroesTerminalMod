using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace KroesTerminal.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        // COMMAND
        [HarmonyPatch("OnSubmit")]
        [HarmonyPrefix]
        private static bool OnSubmitPrefix(Terminal __instance)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            string command = text.ToLower();

            if (command == "kscan")
            {
                KroesTerminal.Log.LogInfo("Custom Command: kscan");
                TerminalNode customNode = Utilities.CreateTerminalNode("kscan");
                __instance.LoadNewNode(customNode);
                return false;
            } else if (command == "kitems")
            {
                KroesTerminal.Log.LogInfo("Custom Command: kitems");
                TerminalNode customNode = Utilities.CreateTerminalNode("kitems");
                __instance.LoadNewNode(customNode);
                return false;
            }
            return true;
        }

        // TEXTPOSTPROCESS
        [HarmonyPatch("TextPostProcess")]
        [HarmonyPrefix]
        private static bool TextPostProcessPrefix(ref string modifiedDisplayText, TerminalNode node, ref string __result, Terminal __instance)
        {
            string keyword = modifiedDisplayText.Trim();

            if (keyword.Contains("[kscan]"))
            {
                KroesTerminal.Log.LogInfo("Processing [kscan]...");
                __result = Utilities.KScanDisplayText();
                Utilities.OnSubmitEnd(__instance);
                return false;
            } else if (keyword.Contains("[kitems]"))
            {
                KroesTerminal.Log.LogInfo("Processing [kitems]...");
                __result = Utilities.KItemsDisplayText();
                Utilities.OnSubmitEnd(__instance);
                return false;
            }
            return true;
        }
    }
}
