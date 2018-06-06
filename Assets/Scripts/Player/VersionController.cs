using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {
	
	private IVersionManager versionManager;

	private void Start() {
		versionManager = GetComponent<InMemoryVersionManager>();	
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)) {
			Debug.Log("Staging current position");
			versionManager.Stage();
		}
		if(Input.GetKeyDown(KeyCode.C)) {
			versionManager.Commit("Committing");
			Debug.Log("Committing Current Position");
		}
		if(Input.GetKeyDown(KeyCode.R)) {
			versionManager.ResetToHead(gameObject);
			Debug.Log("Resetting to Head");
		}
	}
}
