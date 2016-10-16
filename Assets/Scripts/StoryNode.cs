﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryNode : MonoBehaviour {

	public string Dialogue;

	public Speaker DialogueSpeaker;

	public string[] LinkTexts;

	public StoryNode[] NodeLinks;
}

public enum Speaker {
	Plaintiff,
    Witness,
	Defendant
}
