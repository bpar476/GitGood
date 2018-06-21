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
		CommitBuilder builder = new CommitBuilder();
		builder.SetMessage(message);
		builder.SetParent(commitHead);
		foreach(VersionController controller in trackedObjects) {
			int controllerVersion;
			if (stagingArea.Contains(controller)) {
				// increment commit count
				controllerVersion = controller.GenerateVersion();
			}
			else {
				controllerVersion = controller.GetVersion();
			}
			builder.AddObject(controller, controllerVersion);
		}
		ICommit commit = builder.Build();
		commitHead = commit;
		foreach(VersionController stagedController in stagingArea) {
			stagedController.HideStagedState();
		}
		if (activeBranch != null) {
			activeBranch.UpdateTip(commitHead);
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
		if (HasBranch(reference) && branches[reference].GetName().Equals(reference)) {
			Checkout(branches[reference]);
			return true;
		}
		return false;
	}

	public IBranch CreateBranch(string branchName) {
		if (HasBranch(branchName)) {
			throw new System.ArgumentException("Branch already exists");
		}

		IBranch branch = new Branch(branchName, commitHead);
		branches.Add(branchName, branch);

		return branch;
	}

	public bool HasBranch(string branchName) {
		return branches.ContainsKey(branchName);
	}

	public void RefreshGame() {
		ResetToCommit(activeBranch.GetTip());
		// Need to find a way to change branches without restoring to head.
		// Incase users change to branch after making changes and want changes to persist
		// to commit
	}

	public IBranch LookupBranch(string branchName) {
		if (HasBranch(branchName)) {
			return branches[branchName];
		}
		return null;
	}

	public IBranch GetActiveBranch() {
		return activeBranch;
	}

	public ICommit GetHead() {
		return commitHead;
	}
}
