using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;

	public ConeDetector versionableDetector;

	private GameObject currentClosestVersionable;

	private void Start() {
	}

	// Update is called once per frame
	void Update () {
		GameObject closestVersionable = versionableDetector.getClosestDetectedObject();
		highlightClosestVersionableIfPresent(closestVersionable);

		if ((Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
		|| (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))) {
			versionManager.Add(GetComponent<VersionController>());
			Debug.Log("Adding player position");
		} else if(currentClosestVersionable != null && Input.GetKeyDown(KeyCode.Q)) {
			VersionController versionController = currentClosestVersionable.GetComponent<VersionController>();
			versionManager.Add(versionController);
			Debug.Log("Adding closest object");
		} else if(Input.GetKeyDown(KeyCode.E)) {
			versionManager.Commit("Commit message");
			Debug.Log("Commiting staged objects");
		} else if(Input.GetKeyDown(KeyCode.R)) {
			versionManager.ResetToHead();
			Debug.Log("Resetting to HEAD");
		}
	}

	private void toggleOutline(GameObject gobj) {
		SpriteOutline outline = gobj.GetComponent<SpriteOutline>();
		if (outline != null) {
			outline.enabled = !outline.enabled;
		} else {
			Debug.Log("Game object doesn't have outline component");
			Debug.Log(gobj);
		}
	}

	private void highlightClosestVersionableIfPresent(GameObject closestVersionable) {
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftControl)) {
			toggleOutline(gameObject);
			if (currentClosestVersionable != null) {
				toggleOutline(currentClosestVersionable);
			}
		} else {
			if (currentClosestVersionable == null && closestVersionable != null) {
				currentClosestVersionable = closestVersionable;
				toggleOutline(currentClosestVersionable);
			} else if (closestVersionable != null) {
				toggleOutline(currentClosestVersionable);
				currentClosestVersionable = closestVersionable;
				toggleOutline(currentClosestVersionable);
			} else if (currentClosestVersionable != null && closestVersionable == null) {
				toggleOutline(currentClosestVersionable);
				currentClosestVersionable = null;
			}
		}
	}
}
