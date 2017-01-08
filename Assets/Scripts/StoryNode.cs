using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryNode : MonoBehaviour {

	public int Index;

	public string Dialogue;

	public Speaker DialogueSpeaker;

	public string[] LinkTexts;

	public int Points;

	public int[] NodeLinksIndexes;

	public Params[] AdditionalParams;

	public StoryNode[] NodeLinks;
}

public enum Speaker {
	Plaintiff,
    Witness,
	Defendant
}

public enum Params {
	FinalChoice,
	Final
}
