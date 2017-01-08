using UnityEngine;
using System.Collections;

public class CaseFile : MonoBehaviour {

	public TextAsset CaseTable;

	public StoryNode LastNode;

	public int DefendantPoints;
	public int PlaintiffPoints;

	public void AddPoints (int amount) {
		if (amount > 0) {
			DefendantPoints += amount;
		} else if (amount < 0) {
			PlaintiffPoints += Mathf.Abs (amount);
		}
	}
	//public StoryNode PlaintiffGoal;
	//public StoryNode DefendantGoal;
}
