using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplayer : MonoBehaviour {

	public StoryNode StartingNode;

	public GameObject LinkParent;
	public GameObject DialogueParent;

	public GameObject StoryLinkPrefab;
	public GameObject DialoguePrefab;

	//public Text[] Dialogues;
	public GameObject[] Dialogues;

	public Button[] NodeLinks;
	public Text[] LinkTexts;

	void Awake() {
		DisplayNode (StartingNode);	
	}

	public void DisplayNode(StoryNode node) {
		
		for (int i = 0; i < NodeLinks.Length; i++) {
			Destroy(NodeLinks[i].gameObject);
		}

		for (int i = 0; i < Dialogues.Length; i++) {
			Destroy (Dialogues [i].gameObject);
		}

		Dialogues = new GameObject[node.Dialogues.Length];
		NodeLinks = new Button[node.NodeLinks.Length];
		LinkTexts = new Text[node.NodeLinks.Length];

		for (int i = 0; i < Dialogues.Length; i++) {
			GameObject newDialogue = Instantiate (DialoguePrefab, DialogueParent.transform) as GameObject;
			Dialogues [i] = newDialogue;
			newDialogue.GetComponentInChildren<Text> ().text = node.Dialogues [i];
			if (node.DialogueOrientations[i] == Orientation.Left) {
				newDialogue.GetComponent<VerticalLayoutGroup> ().padding.right = 30;
			}
			if (node.DialogueOrientations[i] == Orientation.Right) {
				newDialogue.GetComponent<VerticalLayoutGroup> ().padding.left = 30;
			}
		}

		for (int i = 0; i < NodeLinks.Length; i++) {
			GameObject newStoryLink = Instantiate (StoryLinkPrefab, LinkParent.transform) as GameObject;

			NodeLinks [i] = newStoryLink.GetComponent<Button> ();
			LinkTexts [i] = newStoryLink.GetComponentInChildren<Text> ();
			LinkTexts [i].text = node.LinkTexts [i];
			NodeLinks [i].onClick.RemoveAllListeners ();
			StoryNode displayNode = node.NodeLinks [i];
			NodeLinks [i].onClick.AddListener (delegate {				
				DisplayNode(displayNode);
			});
		}
	}

	void ClickButton(StoryNode node) {
		Debug.Log (node);
		//DisplayNode(node);
	}
}
