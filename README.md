# Terminal workflow

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