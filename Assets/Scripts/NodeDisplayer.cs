using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplayer : MonoBehaviour {

	public StoryNode StartingNode;

	public GameObject LinkParent;
	public GameObject StoryLinkPrefab;

	public Text StoryText;

	public Button[] NodeLinks;
	public Text[] LinkTexts;

	void Awake() {
		DisplayNode (StartingNode);	
	}

	public void DisplayNode(StoryNode node) {

		for (int i = 0; i < NodeLinks.Length; i++) {
			Destroy(NodeLinks[i].gameObject);
		}

		StoryText.text = node.StoryText;
		NodeLinks = new Button[node.NodeLinks.Length];
		LinkTexts = new Text[node.NodeLinks.Length];

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
