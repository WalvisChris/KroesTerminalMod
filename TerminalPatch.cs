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

            if (command == "kscan")
            {
                KroesTerminal.Log.LogInfo("command: kscan");
                
                // LoadNewNode


                return false;
            }

            KroesTerminal.Log.LogInfo("regular command");
            return true;
        }

        [HarmonyPatch("TextPostProcess")]
        [HarmonyPostfix]
        private static void TextProcessPostfix(ref string modifiedDisplayText, TerminalNode node)
        {
            KroesTerminal.Log.LogInfo("TextProcessPostfix: " + modifiedDisplayText);
        }
    }
}
