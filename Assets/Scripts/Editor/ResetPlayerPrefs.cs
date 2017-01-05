using UnityEngine;
using UnityEditor;
using System.Collections;

public class ResetPlayerPrefs : EditorWindow {

	[MenuItem("Edit/Reset Playerprefs")]

	public static void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
