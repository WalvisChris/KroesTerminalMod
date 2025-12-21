# Terminal workflow
user input is stored in Terminal.screenText (TMPro):
```cs
// Create node from input
TerminalNode terminalNode = this.ParsePlayerSentence();
// next step
this.LoadNewNode(terminalNode);
```
`ParsePlayerSentence` returns a TerminalNode:
```cs
return this.terminalNodes.specialNodes[...];
```
`LoadNewNode` sets the final screen text using `TextPostProcess`:
```cs
text = this.TextPostProcess(text, node);
```
How to retreive command from terminal:
```cs
// OnSubmit Prefix
string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
string command = text.ToLower();
```
TerminalNode example:
```cs
// LoadNewNode Prefix
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

// Output
[Info   :KroesTerminal] OnSubmitPrefix: scan
[Info   :KroesTerminal] (string) displayText: [scanForItems]
[Info   :KroesTerminal] (string) terminalEvent:
[Info   :KroesTerminal] (bool) clearPreviousText: True
[Info   :KroesTerminal] (int) maxCharactersToType: 35
[Info   :KroesTerminal] (int) buyItemIndex: -1
[Info   :KroesTerminal] (int) buyVehicleIndex: -1
[Info   :KroesTerminal] (bool) isConfirmationNode: False
[Info   :KroesTerminal] (int) buyRerouteToMoon: -1
[Info   :KroesTerminal] (int) displayPlanetInfo: -1
[Info   :KroesTerminal] (int) shipUnlockableID: -1
[Info   :KroesTerminal] (bool) buyUnlockable: False
[Info   :KroesTerminal] (bool) returnFromStorage: False
[Info   :KroesTerminal] (int) itemCost: 0
[Info   :KroesTerminal] (int) creatureFileID: -1
[Info   :KroesTerminal] (string) creatureName:
[Info   :KroesTerminal] (int) storyLogFileID: -1
[Info   :KroesTerminal] (bool) overrideOptions: False
[Info   :KroesTerminal] (bool) acceptAnything: False
[Info   :KroesTerminal] (CompatibleNoun[]) terminalOptions: 1
[Info   :KroesTerminal] (int) playSyncedClip: -1
[Info   :KroesTerminal] (bool) loadImageSlowly: False
[Info   :KroesTerminal] (bool) persistentImage: False
```

# TextPostProcess.cs
```cs
private string TextPostProcess(string modifiedDisplayText, TerminalNode node)
{
	int num = modifiedDisplayText.Split("[planetTime]", StringSplitOptions.None).Length - 1;
	if (num > 0)
	{
		Regex regex = new Regex(Regex.Escape("[planetTime]"));
		int num2 = 0;
		while (num2 < num && this.moonsCatalogueList.Length > num2)
		{
			Debug.Log(string.Format("isDemo:{0} ; {1}", GameNetworkManager.Instance.isDemo, this.moonsCatalogueList[num2].lockedForDemo));
			string text;
			if (GameNetworkManager.Instance.isDemo && this.moonsCatalogueList[num2].lockedForDemo)
			{
				text = "(Locked)";
			}
			else if (this.moonsCatalogueList[num2].currentWeather == LevelWeatherType.None)
			{
				text = "";
			}
			else
			{
				text = "(" + this.moonsCatalogueList[num2].currentWeather.ToString() + ")";
			}
			modifiedDisplayText = regex.Replace(modifiedDisplayText, text, 1);
			num2++;
		}
	}
	try
	{
		if (node.displayPlanetInfo != -1)
		{
			string text;
			if (StartOfRound.Instance.levels[node.displayPlanetInfo].currentWeather == LevelWeatherType.None)
			{
				text = "mild weather";
			}
			else
			{
				text = (StartOfRound.Instance.levels[node.displayPlanetInfo].currentWeather.ToString().ToLower() ?? "");
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[currentPlanetTime]", text);
		}
	}
	catch
	{
		Debug.Log(string.Format("Exception occured on terminal while setting node planet info; current node displayPlanetInfo:{0}", node.displayPlanetInfo));
	}
	if (modifiedDisplayText.Contains("[warranty]"))
	{
		if (this.hasWarrantyTicket)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[warranty]", "You have a free warranty!");
		}
		else
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[warranty]", "");
		}
	}
	if (modifiedDisplayText.Contains("[currentScannedEnemiesList]"))
	{
		if (this.scannedEnemyIDs == null || this.scannedEnemyIDs.Count <= 0)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[currentScannedEnemiesList]", "No data collected on wildlife. Scans are required.");
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.scannedEnemyIDs.Count; i++)
			{
				Debug.Log(string.Format("scanID # {0}: {1}; {2}", i, this.scannedEnemyIDs[i], this.enemyFiles[this.scannedEnemyIDs[i]].creatureName));
				Debug.Log(string.Format("scanID # {0}: {1}", i, this.scannedEnemyIDs[i]));
				stringBuilder.Append("\n" + this.enemyFiles[this.scannedEnemyIDs[i]].creatureName);
				if (this.newlyScannedEnemyIDs.Contains(this.scannedEnemyIDs[i]))
				{
					stringBuilder.Append(" (NEW)");
				}
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[currentScannedEnemiesList]", stringBuilder.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[buyableItemsList]"))
	{
		if (this.buyableItemsList == null || this.buyableItemsList.Length == 0)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[buyableItemsList]", "[No items in stock!]");
		}
		else
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < this.buyableItemsList.Length; j++)
			{
				stringBuilder2.Append("\n* " + this.buyableItemsList[j].itemName + "  //  Price: $" + ((float)this.buyableItemsList[j].creditsWorth * ((float)this.itemSalesPercentages[j] / 100f)).ToString());
				if (this.itemSalesPercentages[j] != 100)
				{
					stringBuilder2.Append(string.Format("   ({0}% OFF!)", 100 - this.itemSalesPercentages[j]));
				}
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[buyableItemsList]", stringBuilder2.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[buyableVehiclesList]"))
	{
		if (this.buyableVehicles == null || this.buyableVehicles.Length == 0)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[buyableVehiclesList]", "[No items in stock!]");
		}
		else
		{
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int k = 0; k < this.buyableVehicles.Length; k++)
			{
				stringBuilder3.Append("\n* " + this.buyableVehicles[k].vehicleDisplayName + "  //  Price: $" + ((float)this.buyableVehicles[k].creditsWorth * ((float)this.itemSalesPercentages[k + this.buyableItemsList.Length] / 100f)).ToString());
				if (this.itemSalesPercentages[k + this.buyableItemsList.Length] != 100)
				{
					stringBuilder3.Append(string.Format("   ({0}% OFF!)", 100 - this.itemSalesPercentages[k + this.buyableItemsList.Length]));
				}
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[buyableVehiclesList]", stringBuilder3.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[currentUnlockedLogsList]"))
	{
		if (this.unlockedStoryLogs == null || this.unlockedStoryLogs.Count <= 0)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[currentUnlockedLogsList]", "[ALL DATA HAS BEEN CORRUPTED OR OVERWRITTEN]");
		}
		else
		{
			StringBuilder stringBuilder4 = new StringBuilder();
			for (int l = 0; l < this.unlockedStoryLogs.Count; l++)
			{
				stringBuilder4.Append("\n" + this.logEntryFiles[this.unlockedStoryLogs[l]].creatureName);
				if (this.newlyUnlockedStoryLogs.Contains(this.unlockedStoryLogs[l]))
				{
					stringBuilder4.Append(" (NEW)");
				}
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[currentUnlockedLogsList]", stringBuilder4.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[unlockablesSelectionList]"))
	{
		if (this.ShipDecorSelection == null || this.ShipDecorSelection.Count <= 0)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[unlockablesSelectionList]", "[No items available]");
		}
		else
		{
			StringBuilder stringBuilder5 = new StringBuilder();
			for (int m = 0; m < this.ShipDecorSelection.Count; m++)
			{
				stringBuilder5.Append(string.Format("\n{0}  //  ${1}", this.ShipDecorSelection[m].creatureName, this.ShipDecorSelection[m].itemCost));
			}
			modifiedDisplayText = modifiedDisplayText.Replace("[unlockablesSelectionList]", stringBuilder5.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[storedUnlockablesList]"))
	{
		StringBuilder stringBuilder6 = new StringBuilder();
		bool flag = false;
		for (int n = 0; n < StartOfRound.Instance.unlockablesList.unlockables.Count; n++)
		{
			if (StartOfRound.Instance.unlockablesList.unlockables[n].inStorage)
			{
				flag = true;
				stringBuilder6.Append("\n" + StartOfRound.Instance.unlockablesList.unlockables[n].unlockableName);
			}
		}
		if (!flag)
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[storedUnlockablesList]", "[No items stored. While moving an object with B, press X to store it.]");
		}
		else
		{
			modifiedDisplayText = modifiedDisplayText.Replace("[storedUnlockablesList]", stringBuilder6.ToString());
		}
	}
	if (modifiedDisplayText.Contains("[scanForItems]"))
	{
		Random random = new Random(StartOfRound.Instance.randomMapSeed + 91);
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		GrabbableObject[] array = Object.FindObjectsOfType<GrabbableObject>();
		for (int num6 = 0; num6 < array.Length; num6++)
		{
			if (array[num6].itemProperties.isScrap && !array[num6].isInShipRoom && !array[num6].isInElevator)
			{
				num5 += array[num6].itemProperties.maxValue - array[num6].itemProperties.minValue;
				num4 += Mathf.Clamp(random.Next(array[num6].itemProperties.minValue, array[num6].itemProperties.maxValue), array[num6].scrapValue - 6 * num6, array[num6].scrapValue + 9 * num6);
				num3++;
			}
		}
		modifiedDisplayText = modifiedDisplayText.Replace("[scanForItems]", string.Format("There are {0} objects outside the ship, totalling at an approximate value of ${1}.", num3, num4));
	}
	if (this.numberOfItemsInDropship <= 0)
	{
		modifiedDisplayText = modifiedDisplayText.Replace("[numberOfItemsOnRoute]", "");
	}
	else
	{
		modifiedDisplayText = modifiedDisplayText.Replace("[numberOfItemsOnRoute]", string.Format("{0} purchased items on route.", this.numberOfItemsInDropship));
	}
	modifiedDisplayText = modifiedDisplayText.Replace("[currentDay]", DateTime.Now.DayOfWeek.ToString());
	modifiedDisplayText = modifiedDisplayText.Replace("[variableAmount]", this.playerDefinedAmount.ToString());
	modifiedDisplayText = modifiedDisplayText.Replace("[playerCredits]", "$" + this.groupCredits.ToString());
	modifiedDisplayText = modifiedDisplayText.Replace("[totalCost]", "$" + this.totalCostOfItems.ToString());
	modifiedDisplayText = modifiedDisplayText.Replace("[companyBuyingPercent]", string.Format("{0}%", Mathf.RoundToInt(StartOfRound.Instance.companyBuyingRate * 100f)));
	if (this.displayingPersistentImage)
	{
		modifiedDisplayText = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" + modifiedDisplayText;
	}
	return modifiedDisplayText;
}

```
# LoadNewNode.cs
```cs
public void LoadNewNode(TerminalNode node)
{
    this.modifyingText = true;
    this.RunTerminalEvents(node);
    this.screenText.interactable = true;
    string text = "";
    if (node.clearPreviousText)
    {
        text = "\n\n\n" + node.displayText.ToString();
    }
    else
    {
        text = "\n\n" + this.screenText.text.ToString() + "\n\n" + node.displayText.ToString();
        int value = text.Length - 250;
        text = text.Substring(Mathf.Clamp(value, 0, text.Length)).ToString();
    }
    try
    {
        text = this.TextPostProcess(text, node);
    }
    catch (Exception arg)
    {
        Debug.LogError(string.Format("An error occured while post processing terminal text: {0}", arg));
    }
    this.screenText.text = text;
    this.currentText = this.screenText.text;
    this.textAdded = 0;
    if (node.playSyncedClip != -1)
    {
        this.PlayTerminalAudioServerRpc(node.playSyncedClip);
    }
    else if (node.playClip != null)
    {
        this.terminalAudio.PlayOneShot(node.playClip);
    }
    this.LoadTerminalImage(node);
    this.currentNode = node;
}
```

# OnSubmit.cs
```cs
public void OnSubmit()
{
	if (!this.terminalInUse)
	{
		return;
	}
	if (this.textAdded == 0)
	{
		this.screenText.ActivateInputField();
		this.screenText.Select();
		return;
	}
	if (this.currentNode != null && this.currentNode.acceptAnything)
	{
		this.LoadNewNode(this.currentNode.terminalOptions[0].result);
	}
	else
	{
		TerminalNode terminalNode = this.ParsePlayerSentence();
		if (terminalNode != null)
		{
			if (terminalNode.buyRerouteToMoon == -2)
			{
				this.totalCostOfItems = terminalNode.itemCost;
			}
			else if (terminalNode.itemCost != 0)
			{
				this.totalCostOfItems = terminalNode.itemCost * this.playerDefinedAmount;
			}
			if (terminalNode.buyItemIndex != -1 || (terminalNode.buyRerouteToMoon != -1 && terminalNode.buyRerouteToMoon != -2) || terminalNode.shipUnlockableID != -1 || terminalNode.buyVehicleIndex != -1)
			{
				this.LoadNewNodeIfAffordable(terminalNode);
			}
			else if (terminalNode.creatureFileID != -1)
			{
				this.AttemptLoadCreatureFileNode(terminalNode);
			}
			else if (terminalNode.storyLogFileID != -1)
			{
				this.AttemptLoadStoryLogFileNode(terminalNode);
			}
			else
			{
				this.LoadNewNode(terminalNode);
			}
		}
		else
		{
			Debug.Log("load 7");
			this.modifyingText = true;
			this.screenText.text = this.screenText.text.Substring(0, this.screenText.text.Length - this.textAdded);
			this.currentText = this.screenText.text;
			this.textAdded = 0;
		}
	}
	this.screenText.ActivateInputField();
	this.screenText.Select();
	if (this.forceScrollbarCoroutine != null)
	{
		base.StopCoroutine(this.forceScrollbarCoroutine);
	}
	this.forceScrollbarCoroutine = base.StartCoroutine(this.forceScrollbarUp());
}
```

# TerminalNode.cs
```cs
[CreateAssetMenu(menuName = "ScriptableObjects/TerminalNode", order = 2)]
public class TerminalNode : ScriptableObject
{
	[TextArea(2, 20)]
	public string displayText;

	public string terminalEvent;

	[Space(5f)]
	public bool clearPreviousText;

	public int maxCharactersToType = 25;

	[Space(5f)]
	[Header("Purchasing items")]
	public int buyItemIndex = -1;

	public int buyVehicleIndex = -1;

	public bool isConfirmationNode;

	public int buyRerouteToMoon = -1;

	public int displayPlanetInfo = -1;

	[Space(3f)]
	public int shipUnlockableID = -1;

	public bool buyUnlockable;

	public bool returnFromStorage;

	[Space(3f)]
	public int itemCost;

	[Header("Bestiary / Logs")]
	public int creatureFileID = -1;

	public string creatureName;

	public int storyLogFileID = -1;

	[Space(5f)]
	public bool overrideOptions;

	public bool acceptAnything;

	public CompatibleNoun[] terminalOptions;

	[Header("Misc")]
	public AudioClip playClip;

	public int playSyncedClip = -1;

	public Texture displayTexture;

	public VideoClip displayVideo;

	public bool loadImageSlowly;

	public bool persistentImage;
}
```
# ParsePlayerSentence.cs
```cs
private TerminalNode ParsePlayerSentence()
{
    this.broadcastedCodeThisFrame = false;
    string text = this.screenText.text.Substring(this.screenText.text.Length - this.textAdded);
    text = this.RemovePunctuation(text);
    string[] array = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    if (this.currentNode != null && this.currentNode.overrideOptions)
    {
        for (int i = 0; i < array.Length; i++)
        {
            TerminalNode terminalNode = this.ParseWordOverrideOptions(array[i], this.currentNode.terminalOptions);
            if (terminalNode != null)
            {
                return terminalNode;
            }
        }
        return null;
    }
    if (array.Length > 1)
    {
        string a = array[0];
        if (!(a == "switch"))
        {
            if (!(a == "flash"))
            {
                if (!(a == "ping"))
                {
                    if (a == "transmit")
                    {
                        SignalTranslator signalTranslator = Object.FindObjectOfType<SignalTranslator>();
                        if (signalTranslator != null && Time.realtimeSinceStartup - signalTranslator.timeLastUsingSignalTranslator > 8f && array.Length >= 2)
                        {
                            string text2 = text.Substring(8);
                            if (!string.IsNullOrEmpty(text2))
                            {
                                if (!base.IsServer)
                                {
                                    signalTranslator.timeLastUsingSignalTranslator = Time.realtimeSinceStartup;
                                }
                                HUDManager.Instance.UseSignalTranslatorServerRpc(text2.Substring(0, Mathf.Min(text2.Length, 10)));
                                return this.terminalNodes.specialNodes[22];
                            }
                        }
                    }
                }
                else
                {
                    int num = this.CheckForPlayerNameCommand(array[0], array[1]);
                    if (num != -1)
                    {
                        StartOfRound.Instance.mapScreen.PingRadarBooster(num);
                        return this.terminalNodes.specialNodes[21];
                    }
                }
            }
            else
            {
                int num = this.CheckForPlayerNameCommand(array[0], array[1]);
                if (num != -1)
                {
                    StartOfRound.Instance.mapScreen.FlashRadarBooster(num);
                    return this.terminalNodes.specialNodes[23];
                }
                if (StartOfRound.Instance.mapScreen.radarTargets[StartOfRound.Instance.mapScreen.targetTransformIndex].isNonPlayer)
                {
                    StartOfRound.Instance.mapScreen.FlashRadarBooster(StartOfRound.Instance.mapScreen.targetTransformIndex);
                    return this.terminalNodes.specialNodes[23];
                }
            }
        }
        else
        {
            int num = this.CheckForPlayerNameCommand(array[0], array[1]);
            if (num != -1)
            {
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(num);
                return this.terminalNodes.specialNodes[20];
            }
        }
    }
    TerminalKeyword terminalKeyword = this.CheckForExactSentences(text);
    if (terminalKeyword != null)
    {
        if (terminalKeyword.accessTerminalObjects)
        {
            this.CallFunctionInAccessibleTerminalObject(terminalKeyword.word);
            this.PlayBroadcastCodeEffect();
            return null;
        }
        if (terminalKeyword.specialKeywordResult != null)
        {
            return terminalKeyword.specialKeywordResult;
        }
    }
    string value = Regex.Match(text, "\\d+").Value;
    if (!string.IsNullOrWhiteSpace(value))
    {
        this.playerDefinedAmount = Mathf.Clamp(int.Parse(value), 0, 10);
    }
    else
    {
        this.playerDefinedAmount = 1;
    }
    if (array.Length > 5)
    {
        return null;
    }
    TerminalKeyword terminalKeyword2 = null;
    TerminalKeyword terminalKeyword3 = null;
    new List<TerminalKeyword>();
    bool flag = false;
    this.hasGottenNoun = false;
    this.hasGottenVerb = false;
    for (int j = 0; j < array.Length; j++)
    {
        terminalKeyword = this.ParseWord(array[j], 2);
        if (terminalKeyword != null)
        {
            Debug.Log("Parsed word: " + array[j]);
            if (terminalKeyword.isVerb)
            {
                if (this.hasGottenVerb)
                {
                    goto IL_3B9;
                }
                this.hasGottenVerb = true;
                terminalKeyword2 = terminalKeyword;
            }
            else
            {
                if (this.hasGottenNoun)
                {
                    goto IL_3B9;
                }
                this.hasGottenNoun = true;
                terminalKeyword3 = terminalKeyword;
                if (terminalKeyword.accessTerminalObjects)
                {
                    this.broadcastedCodeThisFrame = true;
                    this.CallFunctionInAccessibleTerminalObject(terminalKeyword.word);
                    flag = true;
                }
            }
            if (!flag && this.hasGottenNoun && this.hasGottenVerb)
            {
                break;
            }
        }
        else
        {
            Debug.Log("Could not parse word: " + array[j]);
        }
        IL_3B9:;
    }
    if (this.broadcastedCodeThisFrame)
    {
        this.PlayBroadcastCodeEffect();
        return this.terminalNodes.specialNodes[19];
    }
    this.hasGottenNoun = false;
    this.hasGottenVerb = false;
    if (terminalKeyword3 == null)
    {
        return this.terminalNodes.specialNodes[10];
    }
    if (terminalKeyword2 == null)
    {
        if (!(terminalKeyword3.defaultVerb != null))
        {
            return this.terminalNodes.specialNodes[11];
        }
        terminalKeyword2 = terminalKeyword3.defaultVerb;
    }
    for (int k = 0; k < terminalKeyword2.compatibleNouns.Length; k++)
    {
        if (terminalKeyword2.compatibleNouns[k].noun == terminalKeyword3)
        {
            Debug.Log(string.Format("noun keyword: {0} ; verb keyword: {1} ; result null? : {2}", terminalKeyword3.word, terminalKeyword2.word, terminalKeyword2.compatibleNouns[k].result == null));
            Debug.Log("result: " + terminalKeyword2.compatibleNouns[k].result.name);
            return terminalKeyword2.compatibleNouns[k].result;
        }
    }
    return this.terminalNodes.specialNodes[12];
}
```