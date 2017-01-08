using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject StoryNodePrefab;

	public void UnpackCase (CaseFile caseFile) {
		TextAsset caseTable = caseFile.CaseTable;
		List<GameObject> storyNodeObjects = LoadCase (caseTable);
		int maxIndex = 0;
		foreach (var storyNodeObject in storyNodeObjects) {
			storyNodeObject.transform.SetParent (caseFile.gameObject.transform);
			storyNodeObject.transform.SetSiblingIndex (storyNodeObject.GetComponent<StoryNode> ().Index);
			if (storyNodeObject.GetComponent<StoryNode> ().Index > maxIndex) {
				maxIndex = storyNodeObject.GetComponent<StoryNode> ().Index;
				caseFile.LastNode = storyNodeObject.GetComponent<StoryNode> ();
			}
		}
		Player.instance.TakeCase (caseFile.gameObject);
	}

	public List<GameObject> LoadCase(TextAsset csvTable) {		
		List<GameObject> storyNodeObjects = new List<GameObject> ();
		Dictionary<int, StoryNode> storyNodesByIndexes = new Dictionary<int, StoryNode> (); // Из пушки по воробьям целься...
		string[,] strings = CSVReader.SplitCsvGrid (csvTable.text);
		for (int i = 1; i < strings.GetLength(1) - 1; i++) { // Х - хардкодий
			GameObject newStoryNodeObject = Instantiate (StoryNodePrefab) as GameObject;
			StoryNode newStoryNode = newStoryNodeObject.GetComponent<StoryNode> ();

			newStoryNode.Index = System.Int32.Parse (strings [0, i]);

			newStoryNode.Dialogue = strings [1, i];

			newStoryNode.DialogueSpeaker = (Speaker)System.Enum.Parse (typeof(Speaker), strings [2, i]); // unsafe

			newStoryNode.LinkTexts = strings [3, i].Split (';');

			int number;
			bool result = System.Int32.TryParse(strings [4, i], out number);
			if (result) {
				newStoryNode.Points = number;       
			} else {
				newStoryNode.Points = 0;
			}

			string[] nodeLinksStrings = strings [5, i].Split (';');
			newStoryNode.NodeLinksIndexes = new int[nodeLinksStrings.Length];
			for (int j = 0; j < newStoryNode.NodeLinksIndexes.Length; j++) {
				newStoryNode.NodeLinksIndexes [j] = System.Int32.Parse (nodeLinksStrings [j]);
			}
			storyNodesByIndexes.Add (newStoryNode.Index, newStoryNode); // ...готовься...

			if (strings [6, i] != "") {
				string[] paramsStrings = strings [6, i].Split (';');
				newStoryNode.AdditionalParams = new Params[paramsStrings.Length];
				for (int j = 0; j < paramsStrings.Length; j++) {
					newStoryNode.AdditionalParams [j] = (Params)System.Enum.Parse (typeof(Params), paramsStrings [j]); // unsafe
				}
			}		

			storyNodeObjects.Add (newStoryNodeObject);
		}

		foreach (var storyNodeObject in storyNodeObjects) {
			StoryNode storyNode = storyNodeObject.GetComponent<StoryNode> ();
			storyNode.NodeLinks = new StoryNode[storyNode.NodeLinksIndexes.Length];
			for (int i = 0; i < storyNode.NodeLinks.Length; i++) {
				storyNode.NodeLinks [i] = storyNodesByIndexes [storyNode.NodeLinksIndexes [i]]; // ...пли!
			}
		}

		return storyNodeObjects;
	}
}
