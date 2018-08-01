using System.Collections.Generic;
using UnityEngine;

public class MouseVersionControls : MonoBehaviour {

	public GameObject objectUnderCursor;

	private VersionManager versionManager;

	// Use this for initialization
	void Start () {
		versionManager = VersionManager.Instance();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Fire1") != 0) {
			objectUnderCursor = FindObjectUnderCursor();
			if (objectUnderCursor != null) {
				IMergeWorker mergeWorker = versionManager.GetMergeWorker();
				if (mergeWorker != null && objectUnderCursor.tag == "VersionPreview") {
					mergeWorker.PickObject(objectUnderCursor);
				}
			}
		}
	}

	private GameObject FindObjectUnderCursor() {
		Physics2D.queriesStartInColliders = true;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		GameObject result;
		if (hit.collider != null) {
			result = hit.collider.gameObject;
		} else {
			result = null;
		}
		Physics2D.queriesStartInColliders = false;
		return result;
	}

}
