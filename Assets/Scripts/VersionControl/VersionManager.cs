using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour {

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private ICommit commitHead;
	private List<IBranch> branches;
	private IBranch activeBranch;

	private void Awake() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();
		commitHead = null;

		branches = new List<IBranch>();
		IBranch master = new Branch("master", null);
		branches.Add(master);

		activeBranch = master;
	}

	// Use this for initialization
	void Start () {

	}

	public void Add(VersionController controller) {
		if (!trackedObjects.Contains(controller)) {
			trackedObjects.Add(controller);
		}
		stagingArea.Add(controller);
		controller.StageVersion();
		foreach(VersionController stagedController in stagingArea) {
			stagedController.ShowStagedState();
		}
	}

	public ICommit Commit(string message) {
		ICommit commit = new Commit(commitHead, message);
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
		foreach(VersionController stagedController in stagingArea) {
			stagedController.HideStagedState();
		}
		stagingArea.Clear();

		return commit;
	}

	public void ResetToCommit(ICommit commit) {
		foreach (VersionController trackedObject in trackedObjects) {
			ResetToCommit(commit, trackedObject);
		}
	}

	public void ResetToCommit(ICommit commit, VersionController trackedObject) {
		trackedObject.ResetToVersion(commit.getObjectVersion(trackedObject));
	}

	public void ResetToHead() {
		ResetToCommit(commitHead);
	}
	public void ResetToHead(VersionController versionedObject) {
			ResetToCommit(commitHead, versionedObject);
	}

	public void checkout(IBranch branch) {
		activeBranch = branch;
		commitHead = branch.GetTip();

		RefreshGame();
	}

	public bool checkoutBranch(string reference) {
		foreach (IBranch b in branches) {
			if (b.GetName().Equals(reference)) {
				checkout(b);
				return true;
			}
		}
		return false;
	}

	public void checkout(string reference) {
		if (checkoutBranch(reference)) {
			return;
		}
		// Do other kinds of reference checking such as commit ids, tags, etc;
	}

	public IBranch createBranch(string branchName) {
		IBranch branch = new Branch(branchName, commitHead);
		branches.Add(branch);

		return branch;
	}

	public void RefreshGame() {
		ResetToCommit(activeBranch.GetTip());
	}
}
