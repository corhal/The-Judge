using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public static class Utility {

	public static float SnapNumberToFactor(float number, float factor) {
		int multiple =  Mathf.RoundToInt(number/factor);

		return multiple * factor;
	}

	public static GameObject CastRayToMouse(int layerMask) {		
		Vector3 mousePoint;
		Vector2 mousePoint2D;

		mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePoint2D = new Vector2 (mousePoint.x, mousePoint.y);
		RaycastHit2D hit = Physics2D.Raycast (mousePoint2D, Vector2.zero, 100.0f, layerMask);

		GameObject hitObject = null;

		if (hit.collider != null) {
			hitObject = hit.collider.gameObject;
		}

		return hitObject;
	}

	public static bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		for (int i = 0; i < results.Count; i++) {
			if (results[i].gameObject.GetComponent<Text> () != null) {
				results.RemoveAt (i); // Довольно сомнительное решение, которое я сейчас обосновываю тем, что текстовые лейблы, как правило, некликабельны
			}
		}
		return results.Count > 0;
	}

	// sum=(a+b)(b-a+1)/2

	public static float MathExpectation(float intervalStart, float intervalEnd) {
		float sum = (intervalStart + intervalEnd) * (intervalEnd - intervalStart + 1) / 2;
		return sum / (intervalEnd - intervalStart);
	}

	public static float StringToFloat (string stringToParse) {
		return float.Parse (stringToParse, System.Globalization.CultureInfo.InvariantCulture);
	}

	/*public static IOrderedEnumerable<KeyValuePair<Client, int>> SortDictionary(Dictionary<Client, int> dictionary, bool descending) {
		// Order by values.
		// ... Use LINQ to specify sorting by value.
		IOrderedEnumerable<KeyValuePair<Client, int>> items = null;
		if (!descending) {
			items = from pair in dictionary
				orderby pair.Value ascending
				select pair;
		} else {
			items = from pair in dictionary
				orderby pair.Value descending
				select pair;
		}	

		return items;		
	}*/

	public static Vector3 RotateAroundPivot (Vector3 pivotPosition, Vector3 startPosition, float angle) {
		float rx = startPosition.x - pivotPosition.x;
		float ry = startPosition.z - pivotPosition.z;
		float c = Mathf.Cos(angle);
		float s = Mathf.Sin(angle);
		float x1 = pivotPosition.x + rx * c - ry * s;
		float y1 = pivotPosition.z + rx * s + ry * c;
		return new Vector3 (x1, startPosition.y, y1);
	}

	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	public static string colorToHex(Color32 color) {
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	public static Color hexToColor(string hex) {
		hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a);
	}
}
