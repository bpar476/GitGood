using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMergeConflict : MonoBehaviour {

	public VersionController tutorialBox;
	public VersionManager versionManager;

	public void CreateMergeConflictScenario() {
		versionManager.CreateBranch("conflict-branch");
		versionManager.Checkout("conflict-branch");
		tutorialBox.GetActiveVersion().transform.position = new Vector2(10, tutorialBox.GetActiveVersion().transform.position.y);
		versionManager.Add(tutorialBox);
		versionManager.Commit("move the box");

		versionManager.Checkout("master");
		tutorialBox.GetActiveVersion().transform.position = new Vector2(20, tutorialBox.GetActiveVersion().transform.position.y);
		versionManager.Add(tutorialBox);
		versionManager.Commit("Change the box position");
		
		versionManager.Merge(versionManager.LookupBranch("conflict-branch"));

		tutorialBox.GetActiveVersion().transform.position = new Vector2(15, tutorialBox.GetActiveVersion().transform.position.y);
	}
}
