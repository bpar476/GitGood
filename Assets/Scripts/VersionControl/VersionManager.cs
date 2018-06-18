using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour {

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private ICommit commitHead;
	private IDictionary<string, IBranch> branches;
	private IBranch activeBranch;

	private void Awake() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();
		commitHead = null;

		branches = new Dictionary<string, IBranch>();
		IBranch master = new Branch("master", null);
		branches.Add(master.GetName(), master);

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

	public bool Checkout(string reference) {
		if (CheckoutBranch(reference)) {
			return true;
		}
		// Do other kinds of reference checking such as commit ids, tags, etc;

		return false;
	}

	private void Checkout(IBranch branch) {
		activeBranch = branch;
		commitHead = branch.GetTip();

		RefreshGame();
	}

	public bool CheckoutBranch(string reference) {
		if (branches.ContainsKey(reference) && branches[reference].GetName().Equals(reference)) {
			Checkout(branches[reference]);
			return true;
		}
		return false;
	}

	public IBranch CreateBranch(string branchName) {
		if (branches.ContainsKey(branchName)) {
			throw new System.ArgumentException("Branch already exists");
		}

		IBranch branch = new Branch(branchName, commitHead);
		branches.Add(branchName, branch);

		return branch;
	}

	public void RefreshGame() {
		ResetToCommit(activeBranch.GetTip());
	}
}
