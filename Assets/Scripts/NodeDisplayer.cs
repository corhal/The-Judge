using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplayer : MonoBehaviour {

	public ScrollRect scrollRect;

	public int DialoguePadding;

	public StoryNode StartingNode;
	StoryNode currentNode;

	public GameObject LinkParent;
	public GameObject DialogueParent;

	GameObject nextButtonObject;

	public GameObject StoryLinkPrefab;
	public GameObject DialoguePrefab;

	public List<GameObject> Dialogues;

	public Button[] NodeLinks;
	public Text[] LinkTexts;

	int currentDialogue;

	/*void Awake() {
		DisplayNode (StartingNode);	
	}*/

	void Start() {
		Dialogues = new List<GameObject>();
	}

	public void SetStartingNode(StoryNode startingNode) {
		if (StartingNode == null) {
			StartingNode = startingNode;
			DisplayNode (StartingNode);
		}
	}

	public void DisplayNode(StoryNode node) {

		currentNode = node;

		for (int i = 0; i < NodeLinks.Length; i++) {
			Destroy(NodeLinks[i].gameObject);
		}

		NodeLinks = new Button[node.NodeLinks.Length];
		LinkTexts = new Text[node.NodeLinks.Length];

		DisplayDialogue();
	}

	void DisplayStoryLinks() {
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

	void DisplayDialogue() {
		GameObject newDialogue = Instantiate (DialoguePrefab, DialogueParent.transform) as GameObject;
		Dialogues.Add (newDialogue);
		//Dialogues [index] = newDialogue;
		newDialogue.GetComponentInChildren<Text> ().text = currentNode.Dialogue;

		SetSpeaker (newDialogue, currentNode.DialogueSpeaker);

		if (Dialogues.Count + 1 > currentDialogue) {
			currentDialogue++;
		}

		Debug.Log (currentDialogue + " " + (Dialogues.Count - 1));

		DisplayStoryLinks ();

		scrollRect.verticalNormalizedPosition = 0;
	}

	void ClickButton(StoryNode node) {
		Debug.Log (node);
		//DisplayNode(node);
	}

	void SetSpeaker(GameObject dialogue, Speaker speaker) {
		if (speaker == Speaker.Plaintiff) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.right = DialoguePadding;
			dialogue.GetComponentInChildren<Image> ().color = Color.red;
		}
		if (speaker == Speaker.Witness) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.right = DialoguePadding / 2;
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.left = DialoguePadding / 2;
			dialogue.GetComponentInChildren<Image> ().color = Color.green;
		}
		if (speaker == Speaker.Defendant) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.left = DialoguePadding;
			dialogue.GetComponentInChildren<Image> ().color = Color.blue;
		}
	}
}
