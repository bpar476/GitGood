using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;

	public ConeDetector versionableDetector;

	private SpriteOutline outliner;

	private void Start() {
		outliner = GetComponent<SpriteOutline>();
	}

	// Update is called once per frame
	void Update () {
		GameObject closestVersionable = versionableDetector.getClosestDetectedObject();
		if (Input.GetKey(KeyCode.LeftControl)) {
			outliner.enabled = true;
		} else {
			outliner.enabled = false;
		}

		if ((Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
		|| (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))) {
			versionManager.Add(GetComponent<VersionController>());
			Debug.Log("Adding player position");
		} else if(closestVersionable != null && Input.GetKeyDown(KeyCode.Q)) {
			VersionController versionController = closestVersionable.GetComponent<VersionController>();
			versionManager.Add(versionController);
			Debug.Log("Adding closest object");
		} else if(Input.GetKeyDown(KeyCode.E)) {
			versionManager.Commit("Commit message");
			Debug.Log("Commiting staged objects");
		} else if(Input.GetKeyDown(KeyCode.R)) {
			versionManager.ResetToHead();
			Debug.Log("Resetting to HEAD");
		}

		if(closestVersionable != null) {
			SpriteOutline closestVersionableOutline = closestVersionable.GetComponent<SpriteOutline>();
			if (closestVersionableOutline != null) {
				closestVersionableOutline.enabled = true;
			} else{
				Debug.Log("Closest versionable doesn't have outline component");
			}
		}
	}
}
