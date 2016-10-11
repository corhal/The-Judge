using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryNode : MonoBehaviour {

	public string[] Dialogues;
	public Image[] Images;

	public Speaker[] DialogueSpeakers;

	public string[] LinkTexts;

	public StoryNode[] NodeLinks;
}

public enum Speaker {
	Left,
	Right
}
