using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour {

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private IDictionary<string, IBranch> branches;
	private ICommit activeCommit;
	private IBranch activeBranch;

	private void Awake() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();

		branches = new Dictionary<string, IBranch>();
		IBranch master = new Branch("master", null);
		branches.Add(master.GetName(), master);

		activeBranch = master;
		activeCommit = null;
	}

	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// Adds the given versionable object to the staging area in its
	/// current state.
	/// </summary>
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

	/// <summary>
	/// Creates a new commit from the current state of the staging area.
	/// Appends the commit to the current branch and clears the staging area.
	/// Clears the preview of the staging area.
	/// </summary>
	public ICommit Commit(string message) {
		CommitBuilder builder = new CommitBuilder();
		builder.SetMessage(message);
		builder.SetParent(activeCommit);
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
		activeCommit = commit;
		foreach(VersionController stagedController in stagingArea) {
			stagedController.HideStagedState();
		}
		if (activeBranch != null) {
			activeBranch.UpdateTip(activeCommit);
		}
		stagingArea.Clear();

		return commit;
	}

	// Helper method for setting the state of all tracked objects to the corresponding
	// state in the given commit
	private void LoadStateOfCommit(ICommit commit) {
		foreach (VersionController trackedObject in trackedObjects) {
			LoadStateOfCommit(commit, trackedObject);
		}
	}

	// Helper method for setting the state of the given tracked object to the corresponding
	// state in the given commit
	private void LoadStateOfCommit(ICommit commit, VersionController trackedObject) {
		if (trackedObjects.Contains(trackedObject)) {
			if (commit.ObjectIsTrackedInThisCommit(trackedObject)) {
				trackedObject.ResetToVersion(commit.getObjectVersion(trackedObject));
			} else {
				trackedObject.ResetToInitialState();
			}
		}
	}

	/// <summary>
	/// Resets all tracked objects to the state of the HEAD commit of the current branch
	/// if we are on a branch
	/// </summary>
	public void ResetToHead() {
		LoadStateOfCommit(activeBranch.GetTip());
	}

	/// <summary>
	/// Resets the given versionable object to the state of the HEAD commit of the current
	/// branch if we are on a branch.
	/// </summary>
	public void ResetToHead(VersionController versionedObject) {
		if (activeBranch != null) {
			LoadStateOfCommit(activeBranch.GetTip(), versionedObject);
		}
	}

	/// <summary>
	/// Checks out the commit identified by the given reference.
	/// </summary>
	public bool Checkout(string reference) {
		if (CheckoutBranch(reference)) {
			return true;
		}
		// Do other kinds of reference checking such as commit ids, tags, etc;

		return false;
	}

	/// <summary>
	/// Checks out the specified commit on the specified branch
	/// </summary>
	public void Checkout(IBranch branch, Guid commitId) {
		ICommit commit = branch.GetTip();
		while (! commit.GetCommitId().Equals(commitId)) {
			commit = commit.GetParent();
		}
		LoadStateOfCommit(commit);
		activeCommit = commit;
		activeBranch = branch;
	}

	/// <summary>
	/// Checks out the specified commit on the branch identified by the given reference
	/// </summary>
	public void Checkout(string branchReference, Guid commitId) {
		IBranch branch = LookupBranch(branchReference);
		if (branch != null) {
			Checkout(branch, commitId);
		} else {
			Debug.LogError("The branch: " + branchReference + " does not exist");
		}
	}

	/// <summary>
	/// Checks out the specified commit
	/// </summary>
	public void CheckoutCommit(ICommit commit) {
		if (commit != null) {
			LoadStateOfCommit(commit);
		}
	}

	// Helper function for checking out a branch
	private void Checkout(IBranch branch) {
		activeBranch = branch;
		activeCommit = branch.GetTip();

		RefreshGame();
	}

	/// <summary>
	/// Checks out the commit at the HEAD of the branch specified by the given reference.
	/// </summary>
	public bool CheckoutBranch(string reference) {
		if (HasBranch(reference) && branches[reference].GetName().Equals(reference)) {
			Checkout(branches[reference]);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Creates a new branch from the currently checked out commit or branch HEAD commit and returns it.
	/// Does not check out the newly created branch
	/// </summary>
	public IBranch CreateBranch(string branchName) {
		if (HasBranch(branchName)) {
			throw new System.ArgumentException("Branch already exists");
		}

		IBranch branch = new Branch(branchName, activeCommit);
		branches.Add(branchName, branch);

		return branch;
	}

	/// <summary>
	/// Returns a boolean denoting whether the branch specified by the given reference exists
	/// </summary>
	public bool HasBranch(string branchName) {
		return branches.ContainsKey(branchName);
	}

	/// <summary>
	/// Resets the game state to that of the currently checked out commit.
	/// </summary>
	public void RefreshGame() {
		LoadStateOfCommit(activeCommit);
		// Need to find a way to change branches without restoring to head.
		// Incase users change to branch after making changes and want changes to persist
		// to commit
	}

	/// <summary>
	/// Returns the branch identified by the given reference
	/// </summary>
	public IBranch LookupBranch(string branchName) {
		if (HasBranch(branchName)) {
			return branches[branchName];
		}
		return null;
	}

	/// <summary>
	/// Gets the currently checked out branch
	/// </summary>
	public IBranch GetActiveBranch() {
		return activeBranch;
	}

	/// <summary>
	/// Gets the HEAD commit of the currently checked out branch
	/// </summary>
	public ICommit GetHead() {
		return activeBranch.GetTip();
	}
}
