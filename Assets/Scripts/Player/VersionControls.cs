using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionControls : MonoBehaviour {
	
	public VersionManager versionManager;
	public ConeDetector fov;
	public string versionableTag;
	public string previewTag;

	private GameObject currentlySelectedObject;
	private ClosestObjectVisionObserver closestObjectDetector;

	private IOverlay overlay;

	private void Start() {
		versionManager = GameObject.FindWithTag("VersionManager").GetComponent<VersionManager>();
		closestObjectDetector = GetComponent<ClosestObjectVisionObserver>();
		fov.AddObserver(closestObjectDetector);
		overlay = null;
	}

	// Update is called once per frame
	void Update () {
		if (versionManager.GetMergeWorker() != null) {
			selectClosestPreview();
			if (currentlySelectedObject != null && Input.GetKeyDown(KeyCode.P)) {
				versionManager.GetMergeWorker().PickObject(currentlySelectedObject);
			}
		} else {
			selectVersionable();
			if(currentlySelectedObject != null && Input.GetKeyDown(KeyCode.Q)) {
				VersionController versionController = currentlySelectedObject.GetComponentInParent<VersionController>();
				versionManager.Add(versionController);
				Debug.Log(currentlySelectedObject == gameObject ? "Adding player" : "Adding closest object");
				Debug.Log(currentlySelectedObject);
			} else if (Input.GetKeyDown(KeyCode.E)) {
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
			}
		}

		if(Input.GetKeyDown(KeyCode.O)) {
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
