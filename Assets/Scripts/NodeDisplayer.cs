using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplayer : MonoBehaviour {

	public StoryNode StartingNode;
	StoryNode currentNode;

	public GameObject LinkParent;
	public GameObject DialogueParent;

	GameObject nextButtonObject;

	public GameObject StoryLinkPrefab;
	public GameObject DialoguePrefab;

	public GameObject[] Dialogues;

	public Button[] NodeLinks;
	public Text[] LinkTexts;

	int currentDialogue;

	void Awake() {
		DisplayNode (StartingNode);	
	}

	public void DisplayNode(StoryNode node) {

		currentNode = node;

		for (int i = 0; i < NodeLinks.Length; i++) {
			Destroy(NodeLinks[i].gameObject);
		}

		for (int i = 0; i < Dialogues.Length; i++) {
			if (Dialogues[i] != null) {
				Destroy (Dialogues [i]);
			}
		}

		Dialogues = new GameObject[node.Dialogues.Length];
		NodeLinks = new Button[node.NodeLinks.Length];
		LinkTexts = new Text[node.NodeLinks.Length];
		currentDialogue = 0;

		DisplayDialogue(currentDialogue);
	}

	void DisplayStoryLinks() {
		if (nextButtonObject != null) {
			Destroy (nextButtonObject);
		}
		for (int i = 0; i < NodeLinks.Length; i++) {
			GameObject newStoryLink = Instantiate (StoryLinkPrefab, LinkParent.transform) as GameObject;

			NodeLinks [i] = newStoryLink.GetComponent<Button> ();
			LinkTexts [i] = newStoryLink.GetComponentInChildren<Text> ();
			LinkTexts [i].text = currentNode.LinkTexts [i];
			NodeLinks [i].onClick.RemoveAllListeners ();
			StoryNode displayNode = currentNode.NodeLinks [i];
			NodeLinks [i].onClick.AddListener (delegate {				
				DisplayNode(displayNode);
			});
		}
	}

	void DisplayNextButton() {
		if (nextButtonObject != null) {
			Destroy (nextButtonObject);
		}
		GameObject newStoryLink = Instantiate (StoryLinkPrefab, LinkParent.transform) as GameObject;
		nextButtonObject = newStoryLink;
		Button nextButton = newStoryLink.GetComponent<Button> ();
		Text nextText = newStoryLink.GetComponentInChildren<Text> ();
		nextText.text = "Дальше";
		nextButton.onClick.RemoveAllListeners ();

		nextButton.onClick.AddListener (delegate {				
			DisplayDialogue(currentDialogue);
		});
	}

	void DisplayDialogue(int index) {
		GameObject newDialogue = Instantiate (DialoguePrefab, DialogueParent.transform) as GameObject;
		Dialogues [index] = newDialogue;
		newDialogue.GetComponentInChildren<Text> ().text = currentNode.Dialogues [index];
		if (currentNode.DialogueOrientations[index] == Orientation.Left) {
			newDialogue.GetComponent<VerticalLayoutGroup> ().padding.right = 30;
		}
		if (currentNode.DialogueOrientations[index] == Orientation.Right) {
			newDialogue.GetComponent<VerticalLayoutGroup> ().padding.left = 30;
		}
		if (Dialogues.Length + 1 > currentDialogue) {
			currentDialogue++;
		}

		Debug.Log (currentDialogue + " " + (Dialogues.Length - 1));

		if (currentDialogue < Dialogues.Length) {
			DisplayNextButton ();
		} else {
			DisplayStoryLinks ();
		}
	}

	void ClickButton(StoryNode node) {
		Debug.Log (node);
		//DisplayNode(node);
	}
}
