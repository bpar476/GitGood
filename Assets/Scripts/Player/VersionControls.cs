using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;

	public ConeDetector versionableDetector;

	private void Start() {
	}

	// Update is called once per frame
	void Update () {
		GameObject closestVersionable = versionableDetector.getClosestDetectedObject();
		if ((Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
		|| (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))) {
			versionManager.Add(GetComponentInParent<VersionController>());
			Debug.Log("Adding player position");
		} else if(closestVersionable != null && Input.GetKeyDown(KeyCode.Q)) {
			VersionController versionController = closestVersionable.GetComponentInParent<VersionController>();
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
}
