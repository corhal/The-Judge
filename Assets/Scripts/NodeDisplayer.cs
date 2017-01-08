using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplayer : MonoBehaviour {

	public Slider DefendantSlider;
	public Slider PlaintiffSlider;

	public GameObject WitnessObject;

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

	void Awake () {
		Dialogues = new List<GameObject>();
		DefendantSlider.maxValue = 20.0f; // "Временный" хардкодий
		PlaintiffSlider.maxValue = 20.0f;
	}

	public void SetStartingNode (StoryNode startingNode) {
		if (StartingNode == null) {
			StartingNode = startingNode;
			DisplayNode (StartingNode);
		}
	}

	public void DisplayNode (StoryNode node) {
		currentNode = node;

		for (int i = 0; i < NodeLinks.Length; i++) {
			Destroy(NodeLinks[i].gameObject);
		}

		NodeLinks = new Button[node.NodeLinks.Length];
		LinkTexts = new Text[node.NodeLinks.Length];

		Player.instance.CurrentCase.AddPoints (currentNode.Points);
		DefendantSlider.value = Player.instance.CurrentCase.DefendantPoints;
		PlaintiffSlider.value = Player.instance.CurrentCase.PlaintiffPoints;

		DisplayDialogue ();
	}

	void DisplayDialogue () {
		if (Dialogues.Count > 0) {
			Color currentColor = Dialogues [Dialogues.Count - 1].GetComponentInChildren<Image> ().color;
			Dialogues [Dialogues.Count - 1].GetComponentInChildren<Image> ().color = new Color (currentColor.r, currentColor.g, currentColor.b, 0.5f);
		}	
		GameObject newDialogue = Instantiate (DialoguePrefab, DialogueParent.transform) as GameObject;
		Dialogues.Add (newDialogue);

		if (Utility.ArrayContains (currentNode.AdditionalParams, Params.FinalChoice)) {
			string[] dialogueTexts = currentNode.Dialogue.Split (';');
			if (currentNode.DialogueSpeaker == Speaker.Defendant) {
				if (Player.instance.CurrentCase.PlaintiffPoints > Player.instance.CurrentCase.DefendantPoints) {
					newDialogue.GetComponentInChildren<Text> ().text = dialogueTexts [0];
				} else {
					newDialogue.GetComponentInChildren<Text> ().text = dialogueTexts [1];
				}
			} else {
				if (Player.instance.CurrentCase.PlaintiffPoints < Player.instance.CurrentCase.DefendantPoints) {
					newDialogue.GetComponentInChildren<Text> ().text = dialogueTexts [0];
				} else {
					newDialogue.GetComponentInChildren<Text> ().text = dialogueTexts [1];
				}
			}
		} else {
			newDialogue.GetComponentInChildren<Text> ().text = currentNode.Dialogue;
		}

		SetSpeaker (newDialogue, currentNode.DialogueSpeaker);

		if (Dialogues.Count + 1 > currentDialogue) {
			currentDialogue++;
		}

		DisplayStoryLinks ();

		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = -0.0f;
		Canvas.ForceUpdateCanvases();
	}

	void DisplayStoryLinks () {
		for (int i = 0; i < NodeLinks.Length; i++) {
			GameObject newStoryLink = Instantiate (StoryLinkPrefab, LinkParent.transform) as GameObject;

			NodeLinks [i] = newStoryLink.GetComponent<Button> ();
			LinkTexts [i] = newStoryLink.GetComponentInChildren<Text> ();
			LinkTexts [i].text = currentNode.LinkTexts [i];
			NodeLinks [i].onClick.RemoveAllListeners ();
			StoryNode displayNode = currentNode.NodeLinks [i];

			if (Utility.ArrayContains(currentNode.AdditionalParams, Params.Final)) {
				NodeLinks [i].onClick.AddListener (delegate {				
					Player.instance.GoToOffice();
				});
			} else {
				NodeLinks [i].onClick.AddListener (delegate {				
					DisplayNode(displayNode);
				});
			}
		}
	}

	void SetSpeaker (GameObject dialogue, Speaker speaker) {
		WitnessObject.SetActive (false);
		if (speaker == Speaker.Plaintiff) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.right = DialoguePadding;
			dialogue.GetComponentInChildren<Image> ().color = Color.red;
		}
		if (speaker == Speaker.Witness) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.right = DialoguePadding / 2;
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.left = DialoguePadding / 2;
			dialogue.GetComponentInChildren<Image> ().color = Utility.hexToColor ("006600");
			WitnessObject.SetActive (true);
		}
		if (speaker == Speaker.Defendant) {
			dialogue.GetComponent<VerticalLayoutGroup> ().padding.left = DialoguePadding;
			dialogue.GetComponentInChildren<Image> ().color = Color.blue;
		}
	}

	public void Quit () {
		Application.Quit ();
	}
}
