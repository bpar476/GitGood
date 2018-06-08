using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour {

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private int commitCount = 0;
	private int head = 0;

	private void Awake() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	public void Add(VersionController controller) {
		if (!trackedObjects.Contains(controller)) {
			trackedObjects.Add(controller);
		}
		stagingArea.Add(controller);
		controller.Stage();
	}

	public int Commit(string message) {
		foreach(VersionController controller in stagingArea) {
			controller.Commit(commitCount);
		}
		head = commitCount;
		commitCount++;
		stagingArea.Clear();
		return head;
	}

	public void ResetToCommit(int commitId) {
		foreach(VersionController controller in trackedObjects) {
			controller.ResetToCommit(commitId);
		}
	}

	public void ResetToHead(VersionController versionedObject) {
		ResetToCommit(head);
	}
}
