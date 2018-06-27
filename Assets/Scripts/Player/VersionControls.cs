using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;

	public ConeDetector versionableDetector;

	private GameObject currentSelectedVersionable;

	private IOverlay overlay;

	private void Start() {
		versionManager = GameObject.FindWithTag("VersionManager").GetComponent<VersionManager>();
		overlay = null;
	}

	// Update is called once per frame
	void Update () {
		selectVersionable();

		if(currentSelectedVersionable != null && Input.GetKeyDown(KeyCode.Q)) {
			VersionController versionController = currentSelectedVersionable.GetComponentInParent<VersionController>();
			versionManager.Add(versionController);
			Debug.Log(currentSelectedVersionable == gameObject ? "Adding player" : "Adding closest object");
			Debug.Log(currentSelectedVersionable);
		} else if(versionManager.IsInMergeConflict() && currentSelectedVersionable != null && Input.GetKeyDown(KeyCode.P)) {
			GameObject gameObject = currentSelectedVersionable;
			versionManager.GetMergeWorker().PickObject(gameObject);
		} else if(Input.GetKeyDown(KeyCode.E)) {
			versionManager.Commit("Commit message");
			Debug.Log("Commiting staged objects");
		} else if(Input.GetKeyDown(KeyCode.R)) {
			versionManager.ResetToHead();
			Debug.Log("Resetting to HEAD");
		} else if(Input.GetKeyDown(KeyCode.J)) {
			if (!versionManager.HasBranch("demo")) {
				versionManager.CreateBranch("demo");
				Debug.Log("Creating branch 'demo'");
			}
			versionManager.Checkout("demo");
			Debug.Log("Checkout demo");
		} else if(Input.GetKeyDown(KeyCode.K)) {
			versionManager.Checkout("master");
			Debug.Log("Checkout master");
		} else if(Input.GetKeyDown(KeyCode.O)) {
			if (overlay != null) {
				overlay.Destroy();
				overlay = null;
			}
			else {
				overlay = new Overlay(versionManager.GetHead(), Color.red);
			}
		} else if(Input.GetKeyDown(KeyCode.M)) {
			if (versionManager.GetMergeWorker() != null) {
				versionManager.ResolveMerge();
			}
			else {
				versionManager.Merge(versionManager.LookupBranch("demo"));
			}
		}
	}

	private void selectVersionable() {
		GameObject objectToSelect = null;
		if (Input.GetKey(KeyCode.LeftControl)) {
			objectToSelect =  gameObject;
		} else {
			objectToSelect = versionableDetector.getClosestDetectedObject();
		}

		highlightNewlySelectedVersionableIfPresent(objectToSelect);
		currentSelectedVersionable = objectToSelect;
	}

	private void highlightNewlySelectedVersionableIfPresent(GameObject selectedVersionable) {
		if (currentSelectedVersionable == null && selectedVersionable != null) {
			toggleOutline(selectedVersionable);
		} else if (selectedVersionable != null) {
			toggleOutline(currentSelectedVersionable);
			toggleOutline(selectedVersionable);
		} else if (currentSelectedVersionable != null && selectedVersionable == null) {
			toggleOutline(currentSelectedVersionable);
		}
	}

	private bool toggleOutline(GameObject gobj) {
		SpriteOutline outline = gobj.GetComponent<SpriteOutline>();
		bool result = false;
		if (outline != null) {
			result = outline.enabled;
			outline.enabled = !outline.enabled;
		} else {
			Debug.Log("Game object doesn't have outline component");
			Debug.Log(gobj);
		}
		return result;
	}

	private void setOutline(GameObject gobj, bool enabled) {
		SpriteOutline outline = gobj.GetComponent<SpriteOutline>();
		if (outline != null) {
			outline.enabled = enabled;
		} else {
			Debug.Log("Game object doesn't have outline component");
			Debug.Log(gobj);
		}
	}
}
