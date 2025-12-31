using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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
        public static bool reachedQuota = false;

        public static int totalShipLoot;

        internal static readonly HashSet<string> nonHostileEnemies = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "docile locust bees",
            "manticoil",
            "tulip snake"
        };

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

        internal static bool ShouldListItem(GrabbableObject item)
        {
            if (!item.itemProperties.isScrap) return false;
            if (item is StunGrenadeItem egg) return !egg.hasExploded;
            return true;
        }

        internal static string KScanDisplayText()
        {
            // There are <COUNT> objects outside the ship, totalling a value of $<TOTAL SCRAP VALUE>
            // <LIST>

            string items = "";
            int total = 0;

            GrabbableObject[] array = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            GrabbableObject[] sortedArray = array.Where(obj => ShouldListItem(obj) && !obj.isInShipRoom && !obj.isInElevator).OrderBy(obj => obj.itemProperties.itemName).ToArray();

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
            int heldCount = 0;
            int heldTotal = 0;

            GrabbableObject[] objects = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();

            foreach (GrabbableObject obj in objects)
            {
                if (!ShouldListItem(obj)) continue;
                if (obj.isInShipRoom) 
                {
                    shipCount++;
                    shipTotal += obj.scrapValue;
                }
                else if (obj.isHeld)
                { 
                    heldCount++;
                    heldTotal += obj.scrapValue; 
                }
                else if (obj.isInElevator)
                { 
                    elevatorCount++;
                    elevatorTotal += obj.scrapValue;
                }
                else
                {
                    moonCount++;
                    moonTotal += obj.scrapValue;
                }
            }

            return $"\n\n\n[Items on moon] {moonCount} : ${moonTotal}\n[Items in ship] {shipCount} : ${shipTotal}\n[Items being held] {heldCount} : ${heldTotal}\n[Items on elevator/ship edge] {elevatorCount} : ${elevatorTotal}\n\n";
        }

        private static bool ShouldListEnemy(EnemyAI enemy)
        {
            if (enemy.isEnemyDead) return false;
            if (!KroesPlugin.Configuration.EnemyPeaceful && nonHostileEnemies.Contains(enemy.enemyType.enemyName)) return false;
            return true;
        }

        internal static string KEnemyDisplayText()
        {
            // There are <COUNT> enemies out there.
            // <INSIDE LIST>
            // <OUTSIDE LIST>

            string insideEnemies = "";
            string outsideEnemies = "";

            EnemyAI[] array = UnityEngine.Object.FindObjectsOfType<EnemyAI>();
            EnemyAI[] insideEnemiesArray = array.Where(enemy => !enemy.isOutside && ShouldListEnemy(enemy)).OrderBy(enemy => enemy.enemyType.enemyName).ToArray();
            EnemyAI[] outsideEnemiesArray = array.Where(enemy => enemy.isOutside && ShouldListEnemy(enemy)).OrderBy(enemy => enemy.enemyType.enemyName).ToArray();

            if (insideEnemiesArray.Length == 0 && outsideEnemiesArray.Length == 0) return "\n\n\nNo enemies were found.\n\n";

            foreach (var enemy in insideEnemiesArray)
            {
                string name = "???";

                if (enemy is DressGirlAI ghost)
                {
                    name = ghost.name;
                } 
                else
                {
                    var scanNode = enemy.enemyType.enemyPrefab.GetComponentInChildren<ScanNodeProperties>();
                    name = scanNode?.headerText;
                }

                insideEnemies += $"\n* {name} : {enemy.enemyHP}HP";
            }

            foreach (var enemy in outsideEnemiesArray)
            {
                string name = "???";

                if (enemy is DressGirlAI ghost)
                {
                    name = ghost.name;
                }
                else
                {
                    var scanNode = enemy.enemyType.enemyPrefab.GetComponentInChildren<ScanNodeProperties>();
                    name = scanNode?.headerText;
                }

                outsideEnemies += $"\n* {name} : {enemy.enemyHP}HP";
            }

            int total = insideEnemiesArray.Length + outsideEnemiesArray.Length;
            string result = $"\n\n\nThere are {total} enemies out there.\n";
            if (insideEnemiesArray.Length != 0) result += $"\n[Inside]{insideEnemies}\n";
            if (outsideEnemiesArray.Length != 0) result += $"\n[Outside]{outsideEnemies}\n";
            result += "\n";
            return result;
        }

        internal static int calculateShipLootTotal()
        {
            List<GrabbableObject> list = (from obj in ship.GetComponentsInChildren<GrabbableObject>()
                                          where obj.itemProperties.isScrap && !(obj is RagdollGrabbableObject)
                                          select obj).ToList();
            return list.Sum(scrap => scrap.scrapValue);
        }

        public static void TriggerJump()
        {
            var text = QuotaText;
            if (text == null) return;
            var handler = text.gameObject.AddComponent<TextJumpAnimation>();
            handler.StartJump(text);
        }

        internal static string ConfigPermissionDisplayText(string command)
        {
            return $"\n\n\n[{command}] is disabled in the mod config file.\n\n";
        }

        internal static string KroesDisplayText()
        {
            string result = "\n\n\nThanks for using KroesTerminal!\n\n";
            if (KroesPlugin.Configuration.KScan) result += ">KSCAN\nTo see a detailed scan of all scrap.\n\n";
            if (KroesPlugin.Configuration.KItems) result += ">KITEMS\nTo see a count of all scrap.\n\n";
            if (KroesPlugin.Configuration.KEnemy) result += ">KENEMY\nTo see a scan of all enemies.\n\n";
            if (KroesPlugin.Configuration.KPlayers) result += ">KPLAYERS\nTo see a list of all player items.\n\n";
            return result;
        }

        internal static string PlayersDisplayText()
        {
            PlayerControllerB[] list = StartOfRound.Instance.allPlayerScripts;
            PlayerControllerB[] players = list.Where(player => player.isPlayerControlled && !player.isPlayerDead).ToArray();
            int playerCount = players.Length;
            
            string result = $"\n\n\nThere are {playerCount} players alive:\n";
            
            foreach (PlayerControllerB script in players)
            {
                result += $"\n[{script.playerUsername}]\n";

                GrabbableObject[] items = script.ItemSlots;

                foreach (GrabbableObject item in items)
                {
                    if (item != null) result += $"- {item.itemProperties.itemName}\n";
                }
            }

            result += "\n";
            return result;
        }
    }
}
