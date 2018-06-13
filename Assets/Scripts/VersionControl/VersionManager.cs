using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour {

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private Commit commitHead;

	private void Awake() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();
		commitHead = null;
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

	public Commit Commit(string message) {
		Commit commit = new Commit(commitHead, message);
		foreach(VersionController controller in trackedObjects) {
			int controllerVersion;
			if (stagingArea.Contains(controller)) {
				// increment commit count
				controllerVersion = controller.GenerateVersion();
			}
			else {
				controllerVersion = controller.GetVersion();
			}
			commit.addObject(controller, controllerVersion);
		}
		commitHead = commit;
		stagingArea.Clear();
		return commit;
	}

	public void ResetToCommit(Commit commit) {
		foreach (VersionController trackedObject in trackedObjects) {
			ResetToCommit(commit, trackedObject);
		}
	}

	public void ResetToCommit(Commit commit, VersionController trackedObject) {
		trackedObject.ResetToCommit(commit.getObjectVersion(trackedObject));
	}

	public void ResetToHead() {
		ResetToCommit(commitHead);
	}
	public void ResetToHead(VersionController versionedObject) {
			ResetToCommit(commitHead, versionedObject);
	}
}
