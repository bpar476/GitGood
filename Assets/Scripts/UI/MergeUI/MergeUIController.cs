using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeUIController : MonoBehaviour {

	public RectTransform mergeStatusPanel;
	public RectTransform ResolvedControllersRegion;
	public RectTransform UnresolvedControllersRegion;

	private int numResolved = 0;
	private int numConflicts = 0;

	private IDictionary<GameObject, VersionController> currentConflicts;
	private IDictionary<GameObject, VersionController> currentResolveds;

	public void PopulateConflictObjects(ICollection<VersionController> conflictObjects) {
		float yPos = -20.0f;
		foreach (VersionController conflictedObject in conflictObjects) {
			GameObject uiObj = Instantiate(Resources.Load("UI/MergeUI/UnresolvedObject")) as GameObject;
			MergeObjectController controller = uiObj.GetComponent<MergeObjectController>();
			controller.underlyingObject = conflictedObject;
			RectTransform rTransform = uiObj.GetComponent<RectTransform>();
			rTransform.SetParent(UnresolvedControllersRegion, false);
			rTransform.anchoredPosition = new Vector2(1.0f, yPos);
			yPos -= 20.0f;
		}
	}

	public void ObjectInDictionaryClicked(GameObject gobj) {
	}
}
