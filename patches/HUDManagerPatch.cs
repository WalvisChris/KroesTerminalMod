using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KroesTerminal.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void AwakePostfix(HUDManager __instance)
        {
            GameObject myHotbarUI = new GameObject("myHotbarUI");
            myHotbarUI.transform.SetParent(__instance.HUDContainer.transform, false);
            RectTransform myHotbarRect = myHotbarUI.AddComponent<RectTransform>();
            myHotbarRect.anchorMin = new Vector2(0f, 1f);
            myHotbarRect.anchorMax = new Vector2(0f, 1f);
            myHotbarRect.pivot = new Vector2(0f, 1f);
            myHotbarRect.anchoredPosition = new Vector2(60f, -10f); // POSITION

            GameObject quotaTextGO = new GameObject("QuotaText");
            quotaTextGO.transform.SetParent(myHotbarUI.transform, false);
            TextMeshProUGUI quotaText = quotaTextGO.AddComponent<TextMeshProUGUI>();
            RectTransform rect = quotaTextGO.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, 0f); // POSITION
            rect.sizeDelta = new Vector2(200, 50); // SIZE
            quotaText.font = __instance.controlTipLines[0].font;
            quotaText.fontSize = 20;
            quotaText.fontStyle = FontStyles.Bold;
            quotaText.text = "-placeholder-";
            quotaText.color = Color.white;
            quotaText.alignment = TextAlignmentOptions.Center;
            quotaText.enableWordWrapping = true;
            quotaText.overflowMode = TextOverflowModes.Overflow;
            Utilities.QuotaText = quotaText;
            Utilities.ship = GameObject.Find("/Environment/HangarShip");
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePostfix(HUDManager __instance)
        {
            // [QUOTA] <SHIP TOTAL> : <QUOTA>
            int currentShipLoot = Utilities.ShipLootTotal();
            if (currentShipLoot != Utilities.totalShipLoot)
            {
                Utilities.totalShipLoot = currentShipLoot;
                Utilities.TriggerJump();
            }
            Utilities.QuotaText.text = $"[QUOTA] {Utilities.totalShipLoot} : {TimeOfDay.Instance.profitQuota}";
        }
    }
}
