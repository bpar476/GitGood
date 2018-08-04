using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeUIController : MonoBehaviour {

	public RectTransform mergeStatusPanel;
	public RectTransform ResolvedControllersRegion;
	public RectTransform UnresolvedControllersRegion;

	public void PopulateConflictObjects(ICollection<VersionController> conflictObjects) {
		float yPos = -20.0f;
		foreach (VersionController conflictedObject in conflictObjects) {
			GameObject textObj = Instantiate(Resources.Load("UI/MergeUI/UnresolvedObject")) as GameObject;
			Text text = textObj.GetComponent<Text>();
			text.text = conflictedObject.gameObject.name;
			text.rectTransform.SetParent(UnresolvedControllersRegion, false);
			text.rectTransform.anchoredPosition = new Vector2(1.0f, yPos);
			yPos -= 10.0f;
		}
	}
}
