using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KroesTerminal;
using KroesTerminal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KroesTerminal
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KroesPlugin : BaseUnityPlugin
    {
        private const string GUID = "com.kroes.kroesterminal";
        private const string NAME = "KroesTerminal";
        private const string VERSION = "1.0.7";

        internal static ManualLogSource Log;
        internal static ConfigControl Configuration;
        Harmony harmony = new Harmony(GUID);

        private void Awake()
        {
            Log = Logger;
            Configuration = new ConfigControl(Config);
            harmony.PatchAll(typeof(TerminalPatch));
            if (Configuration.enableQuotaUI || Configuration.QuotaNotif) harmony.PatchAll(typeof(HUDManagerPatch));
            Log.LogInfo("Loaded succesfully!");
        }
    }
}
