using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KroesTerminal
{
    internal class Utilities
    {
        public static TextMeshProUGUI QuotaText;
        public static GameObject ship;

        public static int totalShipLoot;

        internal static TerminalNode CreateTerminalNode(string name)
        {
            // Custom Node
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = $"[{name}]";
            node.terminalEvent = "";
            node.clearPreviousText = true;
            node.maxCharactersToType = 35;
            node.buyItemIndex = -1;
            node.buyVehicleIndex = -1;
            node.isConfirmationNode = false;
            node.buyRerouteToMoon = -1;
            node.displayPlanetInfo = -1;
            node.shipUnlockableID = -1;
            node.buyUnlockable = false;
            node.returnFromStorage = false;
            node.itemCost = 0;
            node.creatureFileID = -1;
            node.creatureName = "";
            node.storyLogFileID = -1;
            node.overrideOptions = false;
            node.acceptAnything = false;
            node.terminalOptions = Array.Empty<CompatibleNoun>();
            node.playSyncedClip = -1;
            node.loadImageSlowly = false;
            node.persistentImage = false;
            return node;
        }

        internal static void OnSubmitEnd(Terminal terminal)
        {
            // Terminal fix
            terminal.screenText.ActivateInputField();
            terminal.screenText.Select();
        }

        internal static string KScanDisplayText()
        {
            // There are <COUNT> objects outside the ship, totalling a value of $<TOTAL SCRAP VALUE>
            // <LIST>

            string items = "";
            int total = 0;

            GrabbableObject[] array = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            GrabbableObject[] sortedArray = array.Where(obj => obj.itemProperties.isScrap && !obj.isInShipRoom && !obj.isInElevator).OrderBy(obj => obj.itemProperties.itemName).ToArray();

            if (sortedArray.Length == 0) return "\n\n\nNo objects were found.\n\n";

            foreach (var obj in sortedArray)
            {
                items += $"\n* {obj.itemProperties.itemName} // ${obj.scrapValue}";
                total += obj.scrapValue;
            }

            return $"\n\n\nThere are {sortedArray.Length} objects outside the ship, totalling a value of ${total}.\n{items}\n\n";
        }

        internal static string KItemsDisplayText()
        {
            // Items on moon: <COUNT> // $<TOTAL SCRAP VALUE>
            // Items in ship: <COUNT> // $<TOTAL SCRAP VALUE>
            // Items in elevator: <COUNT> // $<TOTAL SCRAP VALUE>

            int moonCount = 0;
            int moonTotal = 0;
            int shipCount = 0;
            int shipTotal = 0;
            int elevatorCount = 0;
            int elevatorTotal = 0;

            GrabbableObject[] objects = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();

            foreach(GrabbableObject obj in objects)
            {
                if (!obj.itemProperties.isScrap) continue;
                if (obj.isInShipRoom)
                {
                    shipCount++;
                    shipTotal += obj.scrapValue;
                } else if (!obj.isInElevator)
                {
                    moonCount++;
                    moonTotal += obj.scrapValue;
                } else if (obj.isInElevator)
                {
                    moonCount++;
                    moonTotal += obj.scrapValue;
                }
            }

            return $"\n\n\n[Items on moon] {moonCount} : ${moonTotal}\n[Items in ship] {shipCount} : ${shipTotal}\n[Items in elevator] {elevatorCount} : ${elevatorTotal}\n\n";
        }

        internal static string KEnemyDisplayText()
        {
            // There are <COUNT> enemies out there.
            // <INSIDE LIST>
            // <OUTSIDE LIST>

            string insideEnemies = "";
            string outsideEnemies = "";

            EnemyAI[] array = UnityEngine.Object.FindObjectsOfType<EnemyAI>();
            EnemyAI[] insideEnemiesArray = array.Where(enemy => !enemy.isOutside && !enemy.isEnemyDead).OrderBy(enemy => enemy.enemyType.enemyName).ToArray();
            EnemyAI[] outsideEnemiesArray = array.Where(enemy => enemy.isOutside && !enemy.isEnemyDead).OrderBy(enemy => enemy.enemyType.enemyName).ToArray();

            if (insideEnemiesArray.Length == 0 && outsideEnemiesArray.Length == 0) return "\n\n\nNo enemies were found.\n\n";

            foreach (var enemy in insideEnemiesArray)
            {
                insideEnemies += $"\n* {enemy.enemyType.enemyName} : {enemy.enemyHP}HP";
            }

            foreach (var enemy in outsideEnemiesArray)
            {
                outsideEnemies += $"\n* {enemy.enemyType.enemyName} : {enemy.enemyHP}HP";
            }

            int total = insideEnemiesArray.Length + outsideEnemiesArray.Length;
            string result = $"\n\n\nThere are {total} enemies out there.\n";
            if (insideEnemiesArray.Length != 0) result += $"[Inside]{insideEnemies}\n";
            if (outsideEnemiesArray.Length != 0) result += $"[Outside]{outsideEnemies}\n";
            result += "\n";
            return result;
        }

        internal static int ShipLootTotal()
        {
            List<GrabbableObject> list = (from obj in ship.GetComponentsInChildren<GrabbableObject>()
                                          where obj.itemProperties.isScrap && !(obj is RagdollGrabbableObject)
                                          select obj).ToList();
            return list.Sum(scrap => scrap.scrapValue);
        }

        public static void TriggerJump()
        {
            var text = QuotaText;
            if ( text == null) return;
            var handler = text.gameObject.AddComponent<TextJumpAnimation>();
            handler.StartJump(text);
        }
    }
}
