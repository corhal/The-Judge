using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {

	public static Player instance;
	GameObject caseFileObject;
	public CaseFile CurrentCase;

	public bool hasAchievedDefendantGoal;
	public bool hasAchievedPlaintiffGoal;

	void Awake() {
		//Check if instance already exists
		if (instance == null) {
			//if not, set instance to this
			instance = this;
		}

		//If instance already exists and it's not this:
		else if (instance != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);    
		}
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);	

		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	void SceneManager_sceneLoaded (Scene arg0, LoadSceneMode arg1) {
		if (arg0.buildIndex == 1) {
			CurrentCase = GetComponentInChildren<CaseFile> ();
			StoryNode[] storyNodes = GetComponentsInChildren<StoryNode> ();
			NodeDisplayer displayer = GameObject.FindObjectOfType<NodeDisplayer> ();
			displayer.SetStartingNode (storyNodes[0]);
		}
	}

	public void TakeCase (GameObject newCase) {
		Debug.Log ("Taking case");
		if (CurrentCase != null) {
			Destroy (caseFileObject);
		}
		caseFileObject = Instantiate (newCase);
		caseFileObject.transform.SetParent (transform);

		SceneManager.LoadScene (1);
	}

	public void GoToOffice () {
		SceneManager.LoadScene (0);
	}
}
