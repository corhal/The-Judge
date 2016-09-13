using UnityEngine;
using System.Collections;

public class StoryNode : MonoBehaviour {

	public string[] Dialogues;

	public Orientation[] DialogueOrientations;

	public string[] LinkTexts;

	public StoryNode[] NodeLinks;
}

public enum Orientation {
	Left,
	Right
}
