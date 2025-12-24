using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KroesTerminal
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KroesTerminal : BaseUnityPlugin
    {
        private const string GUID = "com.kroes.kroesterminal";
        private const string NAME = "KroesTerminal";
        private const string VERSION = "1.0.4";

        internal static ManualLogSource Log;
        Harmony harmony = new Harmony(GUID);

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo("Loaded succesfully!");
            harmony.PatchAll();
        }
    }
}
