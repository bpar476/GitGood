using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	public ConeDetector fov;
	public string versionableTag;
	public string previewTag;

	private GameObject currentlySelectedObject;
	private ClosestObjectVisionObserver closestObjectDetector;

	private IOverlay overlay;

	private void Start() {
		closestObjectDetector = GetComponent<ClosestObjectVisionObserver>();
		fov.AddObserver(closestObjectDetector);
		overlay = null;
	}

	// Update is called once per frame
	void Update () {
		if (VersionManager.Instance().GetMergeWorker() != null) {
			selectClosestPreview();
			if (currentlySelectedObject != null && Input.GetKeyDown(KeyCode.P)) {
				VersionManager.Instance().GetMergeWorker().PickObject(currentlySelectedObject);
			}
		} else {
			selectVersionable();
			if(currentlySelectedObject != null && Input.GetKeyDown(KeyCode.Q)) {
				VersionController versionController = currentlySelectedObject.GetComponentInParent<VersionController>();
				VersionManager.Instance().Add(versionController);
				Debug.Log(currentlySelectedObject == gameObject ? "Adding player" : "Adding closest object");
				Debug.Log(currentlySelectedObject);
			} else if (Input.GetKeyDown(KeyCode.E)) {
				VersionManager.Instance().Commit("Commit message");
				Debug.Log("Commiting staged objects");
			} else if(Input.GetKeyDown(KeyCode.R)) {
				VersionManager.Instance().ResetToHead();
				Debug.Log("Resetting to HEAD");
			} else if(Input.GetKeyDown(KeyCode.J)) {
				if (!VersionManager.Instance().HasBranch("demo")) {
					VersionManager.Instance().CreateBranch("demo");
					Debug.Log("Creating branch 'demo'");
				}
				VersionManager.Instance().Checkout("demo");
				Debug.Log("Checkout demo");
			} else if(Input.GetKeyDown(KeyCode.K)) {
				VersionManager.Instance().Checkout("master");
				Debug.Log("Checkout master");
			}
		}

		if(Input.GetKeyDown(KeyCode.O)) {
			if (overlay != null) {
				overlay.Destroy();
				overlay = null;
			}
			else {
				overlay = new Overlay(VersionManager.Instance().GetHead(), Color.red);
			}
		} else if(Input.GetKeyDown(KeyCode.M)) {
			if (VersionManager.Instance().GetMergeWorker() != null) {
				VersionManager.Instance().ResolveMerge();
			}
			else {
				VersionManager.Instance().Merge(VersionManager.Instance().LookupBranch("demo"));
			}
		}
	}

	private void selectVersionable() {
		GameObject objectToSelect = null;
		if (Input.GetKey(KeyCode.LeftControl)) {
			objectToSelect =  gameObject;
		} else {
			objectToSelect = closestObjectDetector.GetClosestObjectWithTag(this.versionableTag);
		}

		HighlightNewlySelectedObjectIfPresent(objectToSelect);
		currentlySelectedObject = objectToSelect;
	}

	private void selectClosestPreview() {
		GameObject closestPreview = this.closestObjectDetector.GetClosestObjectWithTag(this.previewTag);
		HighlightNewlySelectedObjectIfPresent(closestPreview);
		currentlySelectedObject = closestPreview;
	}

	private void HighlightNewlySelectedObjectIfPresent(GameObject selectedObject) {
		if (currentlySelectedObject == null && selectedObject != null) {
			toggleOutline(selectedObject);
		} else if (selectedObject != null) {
			toggleOutline(currentlySelectedObject);
			toggleOutline(selectedObject);
		} else if (currentlySelectedObject != null && selectedObject == null) {
			toggleOutline(currentlySelectedObject);
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
