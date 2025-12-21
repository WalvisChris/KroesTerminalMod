# Terminal workflow

# TerminalNode
```cs
[CreateAssetMenu(menuName = "ScriptableObjects/TerminalNode", order = 2)]
public class TerminalNode : ScriptableObject
{
	// Token: 0x04001302 RID: 4866
	[TextArea(2, 20)]
	public string displayText;

	// Token: 0x04001303 RID: 4867
	public string terminalEvent;

	// Token: 0x04001304 RID: 4868
	[Space(5f)]
	public bool clearPreviousText;

	// Token: 0x04001305 RID: 4869
	public int maxCharactersToType = 25;

	// Token: 0x04001306 RID: 4870
	[Space(5f)]
	[Header("Purchasing items")]
	public int buyItemIndex = -1;

	// Token: 0x04001307 RID: 4871
	public int buyVehicleIndex = -1;

	// Token: 0x04001308 RID: 4872
	public bool isConfirmationNode;

	// Token: 0x04001309 RID: 4873
	public int buyRerouteToMoon = -1;

	// Token: 0x0400130A RID: 4874
	public int displayPlanetInfo = -1;

	// Token: 0x0400130B RID: 4875
	[Space(3f)]
	public int shipUnlockableID = -1;

	// Token: 0x0400130C RID: 4876
	public bool buyUnlockable;

	// Token: 0x0400130D RID: 4877
	public bool returnFromStorage;

	// Token: 0x0400130E RID: 4878
	[Space(3f)]
	public int itemCost;

	// Token: 0x0400130F RID: 4879
	[Header("Bestiary / Logs")]
	public int creatureFileID = -1;

	// Token: 0x04001310 RID: 4880
	public string creatureName;

	// Token: 0x04001311 RID: 4881
	public int storyLogFileID = -1;

	// Token: 0x04001312 RID: 4882
	[Space(5f)]
	public bool overrideOptions;

	// Token: 0x04001313 RID: 4883
	public bool acceptAnything;

	// Token: 0x04001314 RID: 4884
	public CompatibleNoun[] terminalOptions;

	// Token: 0x04001315 RID: 4885
	[Header("Misc")]
	public AudioClip playClip;

	// Token: 0x04001316 RID: 4886
	public int playSyncedClip = -1;

	// Token: 0x04001317 RID: 4887
	public Texture displayTexture;

	// Token: 0x04001318 RID: 4888
	public VideoClip displayVideo;

	// Token: 0x04001319 RID: 4889
	public bool loadImageSlowly;

	// Token: 0x0400131A RID: 4890
	public bool persistentImage;
}
```