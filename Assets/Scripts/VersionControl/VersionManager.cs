using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : Singleton<VersionManager> {
	protected override void Awake() {
		base.Awake();

		//Construct Initial Commit
		CommitBuilder cb = new CommitBuilder();
		cb.SetMessage("Initial Commit");
		ICommit initialCommit = cb.Build();
		activeCommit = initialCommit;
		activeBranch.UpdateTip(initialCommit);
	}

	public TriggerManager mergeTrigger;
	public TriggerManager commitTrigger;
	public TriggerManager addTrigger;
	public TriggerManager unstageTrigger;
	public TriggerManager branchTrigger;
	public TriggerManager checkoutTrigger;
	public TriggerManager pickTrigger;

	public MergeUIController mergeUI;

	IList<VersionController> trackedObjects;
	IList<VersionController> stagingArea;
	private IDictionary<string, IBranch> branches;
	private ICommit activeCommit;
	private IBranch activeBranch;
	private bool isDetached;

	private VersionController lastStagedObject;
	private VersionController lastUnstagedObject;

	private IBranch lastCreatedBranch;

	private IMergeWorker mw;

	private VersionManager() {
		trackedObjects = new List<VersionController>();
		stagingArea = new List<VersionController>();

		branches = new Dictionary<string, IBranch>();
		IBranch master = new Branch("master", null);
		branches.Add(master.GetName(), master);

		activeBranch = master;
		activeCommit = null;
		isDetached = false;
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

		lastStagedObject = controller;

		if (addTrigger != null) {
			addTrigger.Trigger();
		}

		UIController.Instance().UpdateOverlay();
	}

	public void Add(VersionController controller, IVersion version) {
		if (!trackedObjects.Contains(controller)) {
			trackedObjects.Add(controller);
		}
		stagingArea.Add(controller);
		controller.StageVersion(version);
		foreach(VersionController stagedController in stagingArea) {
			stagedController.ShowStagedState();
		}

		lastStagedObject = controller;
		if (addTrigger != null) {
			addTrigger.Trigger();
		}

		UIController.Instance().UpdateOverlay();
	}

	public VersionController GetLastStagedObject() {
		if (stagingArea.Contains(lastStagedObject)) {
			return lastStagedObject;
		} else {
			return null;
		}
	}

	/// <summary>
	/// Removes the given versionable object from the staging area
	/// </summary>
	/// <param name="controller">The versionable object to remove from the staging area</param>
	public void Unstage(VersionController controller) {
		if (stagingArea.Contains(controller)) {
			stagingArea.Remove(controller);
			controller.HideStagedState();
			lastUnstagedObject = controller;

			if (unstageTrigger != null) {
				unstageTrigger.Trigger();
			}
		}

		UIController.Instance().UpdateOverlay();
	}

	public List<String> GetStagedObjectNames() {
		List<String> objectNames = new List<String>();
		foreach (VersionController vc in stagingArea) {
			objectNames.Add(vc.objectName);
		}
		return objectNames;
		
	}

	public VersionController GetLastUnstagedObject() {
		return lastUnstagedObject;
	}

	/// <summary>
	/// Wrapper method for commit API. Checks if player is able to commit yet.
	/// </summary>
	/// <param name="message">The commit message</param>
	/// <returns>The commit if the player is able to commit, null otherwise</returns>
	public ICommit Commit(string message) {
		if (!EnabledVersionControls.Instance().CanCommit) {
			Debug.Log("Can't Commit; not enabled yet");
			return null;
		} else {
			return this.Commit(message, false);
		}
	}

	/// <summary>
	/// Creates a new commit from the current state of the staging area.
	/// Appends the commit to the current branch and clears the staging area.
	/// Clears the preview of the staging area.
	/// 
	/// Additional parameter for forcing commits through the player's enabled version control mechanics
	/// such as for oneShot doors.
	/// </summary>
	public ICommit Commit(string message, bool forced) {
		if (!EnabledVersionControls.Instance().CanCommit && !forced) {
			Debug.Log("Can't Commit; not enabled yet");
			return null;
		}
		if (isDetached) {
			throw new InvalidOperationException("Cannot commit in detached HEAD state");
		}

		CommitBuilder builder = new CommitBuilder();
		builder.SetMessage(message);
		builder.SetParent(activeCommit);
		foreach(VersionController controller in trackedObjects) {
			IVersion controllerVersion;
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

		if (commitTrigger != null) {
			commitTrigger.Trigger();
		}

		UIController.Instance().UpdateOverlay();

		return commit;
	}

	// Helper method for setting the state of all tracked objects to the corresponding
	// state in the given commit
	private void LoadStateOfCommit(ICommit commit) {
		for (int i = 0; i < trackedObjects.Count; i++) {
			LoadStateOfCommit(commit, trackedObjects[i]);
		}
		IEnumerator<VersionController> objectsTrackedInCommit = commit.GetTrackedObjectsEnumerator();
		while (objectsTrackedInCommit.MoveNext()) {
			LoadStateOfCommit(commit, objectsTrackedInCommit.Current);
		}
	}

	private void LoadStateOfCommit(ICommit commit, VersionController versionableObject) {
		int trackedObjectIndex = trackedObjects.IndexOf(versionableObject);
		if (trackedObjectIndex != -1) {
			LoadStateOfCommitForTrackedObject(commit, versionableObject, trackedObjectIndex);
		} else {
			LoadStateOfCommitForNewTrackedObject(commit, versionableObject);
		}
	}

	// Helper method for setting the state of the given tracked object to the corresponding
	// state in the given commit
	private void LoadStateOfCommitForTrackedObject(ICommit commit, VersionController trackedObject, int trackedObjectIndex) {
		if (commit.ObjectIsTrackedInThisCommit(trackedObject)) {
			trackedObject.ResetToVersion(commit.getObjectVersion(trackedObject));
		} else {
			trackedObject.ResetToInitialState();
			trackedObjects.RemoveAt(trackedObjectIndex);
		}
	}

	// Helper method for adding a new object to the tracked objects when loading a commit where the object is tracked but was not
	private void LoadStateOfCommitForNewTrackedObject(ICommit commit, VersionController versionableObject) {
		trackedObjects.Add(versionableObject);
		versionableObject.ResetToVersion(commit.getObjectVersion(versionableObject));
	}

	/// <summary>
	/// Resets all tracked objects to the state of the HEAD commit of the current branch
	/// if we are on a branch
	/// </summary>
	public void ResetToHead() {
		if (!EnabledVersionControls.Instance().CanReset) {
			Debug.Log("Can't Reset; not enabled yet");
			return;
		}
		LoadStateOfCommit(activeBranch.GetTip());
	}

	/// <summary>
	/// Resets the given versionable object to the state of the HEAD commit of the current
	/// branch if we are on a branch.
	/// </summary>
	public void ResetToHead(VersionController versionedObject) {
		if (!EnabledVersionControls.Instance().CanReset) {
			Debug.Log("Can't Reset; not enabled yet");
			return;
		} else if (activeBranch != null) {
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
	/// Checks out the commit specified by the given commit ID on the specified branch
	/// </summary>
	public void Checkout(IBranch branch, Guid commitId) {
		ICommit commit = branch.GetTip();
		while (! commit.GetId().Equals(commitId)) {
			commit = commit.GetParent();
		}
		Checkout(branch, commit);
	}

	/// <summary>
	/// Checks out the given commit on the given branch
	/// </summary>
	public void Checkout(IBranch branch, ICommit commit) {
		// Need to allow them to checkout the last created branch because that's how "create branch" works
		if (!EnabledVersionControls.Instance().CanCheckout && !lastCreatedBranch.Equals(branch)) {
			Debug.Log("Can't Checkout branch; not enabled yet");
			return;
		}
		LoadStateOfCommit(commit);
		activeCommit = commit;
		activeBranch = branch;
		if (! activeCommit.Equals(activeBranch.GetTip())) {
			this.isDetached = true;
		} else {
			this.isDetached = false;
		}

		if (checkoutTrigger != null) {
			checkoutTrigger.Trigger();
		}

		UIController.Instance().UpdateOverlay();
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
	public void Checkout(IBranch branch) {
		Checkout(branch, branch.GetTip());
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
		if (!EnabledVersionControls.Instance().CanBranch) {
			Debug.Log("Can't Branch; not enabled yet");
			return null;
		}
		if (HasBranch(branchName)) {
			throw new System.ArgumentException("Branch already exists");
		}

		IBranch branch = new Branch(branchName, activeCommit);
		branches.Add(branchName, branch);

		if (branchTrigger != null) {
			branchTrigger.Trigger();
		}

		lastCreatedBranch = branch;

		return branch;
	}

	/// <summary>
	/// Returns a boolean denoting whether the branch specified by the given reference exists
	/// </summary>
	public bool HasBranch(string branchName) {
		return branches.ContainsKey(branchName);
	}
	
	/// <summary>
	/// Returns a collection of all the branch names
	/// </summary>
	/// <returns></returns>
	public ICollection<string> GetBranchList() {
		return branches.Keys;
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
	/// Gets the currently checked out commit
	/// </summary>
	public ICommit GetActiveCommit() {
		return activeCommit;
	}

	/// <summary>
	/// Gets the HEAD commit of the currently checked out branch
	/// </summary>
	public ICommit GetHead() {
		return activeBranch.GetTip();
	}

	/// <summary>
	/// Determines whether the given object is currently tracked
	/// </summary>
	public bool IsObjectTracked(VersionController controller) {
		return trackedObjects.Contains(controller);
	}

	/// <summary>
	/// Determines whether a given versionable object is in the staging area
	/// </summary>
	/// <param name="controller">The versionable object to query the staging area for</param>
	/// <returns>True if the object is in the staging area, false otherwise.</returns>
	public bool IsObjectStaged(VersionController controller) {
		return stagingArea.Contains(controller);
	}
	
	#region Merging
	public Relationship Merge(IBranch featureBranch) {
		if (!EnabledVersionControls.Instance().CanMerge) {
			Debug.Log("Merging not enabled yet");
		}
		if (isDetached) {
			throw new Exception("Can't merge if detached");
		}

		if (this.mw != null) {
			throw new Exception("Already doing a merge, resolve this first");
		}

		IMergeWorker mw = new MergeWorker(activeBranch, featureBranch, pickTrigger, mergeUI);
		Relationship mergeType = mw.GetMergeType();

		if (mw.GetMergeType() == Relationship.FastForward) {
			mw.End();
			activeBranch.UpdateTip(featureBranch.GetTip());
			activeCommit = activeBranch.GetTip();
			LoadStateOfCommit(activeCommit);

			if (mergeTrigger != null) {
				mergeTrigger.Trigger();
			}

			return mergeType;
		}

		this.mw = mw;

		return mergeType;
	}

	public bool IsInMergeConflict() {
		return this.mw != null && !this.mw.IsResolved();
	}

	public ICommit ResolveMerge() {
		if (this.mw == null) {
			throw new Exception("Not in merge...");
		}
		if (!this.mw.IsResolved()) {
			return null;
		}

		if (Camera.main != null) {
			Camera.main.GetComponent<MergeInterfaceCamera>().enabled = false;
		}

		return CreateMergeCommit("Merge Commit");
	}

	public ICommit CreateMergeCommit(string commitMessage) {
		foreach (KeyValuePair<VersionController, IVersion> stageData in this.mw.BuildStagingArea()) {
			this.Add(stageData.Key, stageData.Value);
		}
		ICommit mergeCommit = this.Commit(commitMessage);
		foreach (VersionController versionedObject in this.mw.BuildStagingArea().Keys) {
			this.ResetToHead(versionedObject);
		}

		this.mw.End();
		this.mw = null;

		if (mergeTrigger != null) {
			mergeTrigger.Trigger();
		}


		return mergeCommit;
	}

	public IMergeWorker GetMergeWorker() {
		return this.mw;
	}

	#endregion
}
