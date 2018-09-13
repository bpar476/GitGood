using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeObjectController : MonoBehaviour {

	public VersionController underlyingObject {

		get {
			return _underlyingObject;
		}

		set {
			_underlyingObject = value;
			transform.Find("Object").GetComponent<Text>().text = value.objectName;
		}
	}
	private VersionController _underlyingObject;

	public string baseBranch {
		
		get {
			return this._baseBranch;
		}

		set {
			this._baseBranch = value;
			transform.Find("Version 1").GetComponent<Text>().text = value;
		}
	}
	private string _baseBranch;

	public string featureBranch {
		
		get {
			return this._featureBranch;
		}

		set {
			this._featureBranch = value;
			transform.Find("Version 2").GetComponent<Text>().text = value;
		}
	}
	private string _featureBranch;

	private GameObject basePreview;
	private GameObject featurePreview;

	private bool isResolved;

	public bool IsResolved() {
		return isResolved;
	}

	public void SetBasePreview(GameObject preview) {
		this.basePreview = preview;
		Color previewColor = preview.GetComponent<SpriteRenderer>().color;
		Color textColor = new Color(previewColor.r, previewColor.g, previewColor.b, 1);
		transform.Find("Version 1").GetComponent<Text>().color = textColor;
	}

	public void SetFeaturePreview(GameObject preview) {
		this.featurePreview = preview;
		Color previewColor = preview.GetComponent<SpriteRenderer>().color;
		Color textColor = new Color(previewColor.r, previewColor.g, previewColor.b, 1);
		transform.Find("Version 2").GetComponent<Text>().color = textColor;
	}

	public void BaseBranchTextClicked() {
		this.BranchTextClicked(this.basePreview);
	}

	public void FeatureBranchTextClicked() {
		this.BranchTextClicked(this.featurePreview);
	}

	private void BranchTextClicked(GameObject preview) {
		Camera.main.transform.position = new Vector3(preview.transform.position.x, preview.transform.position.y, Camera.main.transform.position.z);
		Camera.main.orthographicSize = 5;
	}
	
	public void VersionWasPicked(GameObject versionPicked) {
		if (versionPicked.Equals(basePreview)) {
			UpdatePickedVersion(transform.Find("Version 1").GetComponent<Text>(), transform.Find("Version 2").GetComponent<Text>());
		} else if (versionPicked.Equals(featurePreview)) {
			UpdatePickedVersion(transform.Find("Version 2").GetComponent<Text>(), transform.Find("Version 1").GetComponent<Text>());
		}
	}

	private void UpdatePickedVersion(Text pickedVersion, Text notPickedVersion) {
		pickedVersion.color = Color.green;
		notPickedVersion.color = Color.gray;
		isResolved = true;
	}
}
