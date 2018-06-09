using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;

	private void Start() {
	}

	// Update is called once per frame
	void Update () {
		VersionController versionable = findNearestVersionableObject();
	}

	private VersionController findNearestVersionableObject() {
		return null;
	}


}
