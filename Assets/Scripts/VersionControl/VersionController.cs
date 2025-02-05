﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<IVersionable> versioners;

	public string objectName;

	public GameObject templatePrefab;
	public GameObject previewPrefab;
	public Transform initialPosition;
	public TransformVersionable transformVersioner;

	private GameObject activeVersion;
	private IDictionary<IVersion, GameObject> previewVersions;
	private GameObject stagedStatePreview;
	StagedObjectPreviewController stagedUIController;

	private IVersion version = new Version();

	private void Awake() {
		versioners = new List<IVersionable>();
		if (transformVersioner != null) {
			versioners.Add(transformVersioner);
		}
	}

	// Use this for initialization
	void Start () {
		if (activeVersion == null) {
			activeVersion = Instantiate(templatePrefab, transform) as GameObject;
			activeVersion.transform.position = initialPosition.position;
		}
		if (initialPosition == null) {
			initialPosition = new GameObject().transform;
		}

		VersionManager.Instance().Add(this);
		VersionManager.Instance().Commit("Initial Commit for " + this.objectName, true);
	}

	#region accessors
	public void SetActiveVersion(GameObject gobj) {
		activeVersion = gobj;
	}

	public GameObject GetActiveVersion() {
		return activeVersion;
	}

	public void SetTemplatePrefab(GameObject template) {
		templatePrefab = template;
	}

	public void SetPreviewPrefab(GameObject preview) {
		previewPrefab = preview;
	}

	public void SetInitialPosition(float x, float y) {
		if (initialPosition == null) {
			initialPosition = new GameObject().transform;
		}
		initialPosition.position = new Vector2(x, y);
	}
	#endregion accessors

	public string DescribeState(IVersion version) {
		string result = "";
		foreach(IVersionable versioner in versioners) {
			result += versioner.DescribeState(version) +"\n";
		}
		return result;
	}

	public string DescribeStagedState() {
		string result = "";
		foreach(IVersionable versioner in versioners) {
			result += versioner.DescribeStagedState() +"\n";
		}
		return result;
	}

	public void StageVersion() {
		foreach (IVersionable versioner in versioners) {
			versioner.Stage(activeVersion);
		}
	}

	public void StageVersion(IVersion version) {
		GameObject gameObject = this.ReconstructVersion(version);
		foreach (IVersionable versioner in versioners) {
			versioner.Stage(gameObject);
		}
		Destroy(gameObject);
	}

	public IVersion GenerateVersion() {
		version = new Version(version);
		foreach (IVersionable versioner in versioners) {
			versioner.Commit(version);
		}
		return version;
	}

	public IVersion GetVersion() {
		return this.version;
	}

	public void ResetToVersion(IVersion version) {
		this.ResetToVersion(version, activeVersion);
		this.version = version;
	}

	public void ResetToVersion(IVersion version, GameObject gameObject) {
		foreach (IVersionable versioner in versioners) {
			versioner.ResetToVersion(version, gameObject);
		}
	}

	public void ResetToInitialState() {
		foreach (IVersionable versioner in versioners) {
			versioner.ResetToInitialState(activeVersion);
		}
	}

	public void AddVersionable(IVersionable versioner) {
		if (versioner != null) {
			versioners.Add(versioner);
		}
	}


	public void ShowVersion(IVersion version) {
		GameObject preview;
		if(!previewVersions.TryGetValue(version, out preview)) {
			preview = Instantiate(previewPrefab, transform) as GameObject;
			this.ResetToVersion(version, preview);
			SpriteRenderer renderer = preview.GetComponent<SpriteRenderer>();
			renderer.color = new Color(0.5f, 0.1f, 0.0f, 0.4f);
			previewVersions.Add(version, preview);
		}
	}

	public GameObject ReconstructVersion(IVersion version) {
		GameObject gameObject = Instantiate(previewPrefab, transform) as GameObject;
		this.ResetToVersion(version, gameObject);

		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.color = new Color(0.5f, 0.1f, 0.0f, 0.4f);

		return gameObject;
	}

	public void ShowStagedState() {
		if(stagedStatePreview == null) {
			stagedStatePreview =  Instantiate(previewPrefab, transform) as GameObject;
			GameObject uiObject = Instantiate(Resources.Load("UI/StagedPreviewSummary")) as GameObject;
			stagedUIController = uiObject.GetComponent<StagedObjectPreviewController>();
			stagedUIController.SetPreviewObject(this, stagedStatePreview);
		}
		foreach (IVersionable versioner in versioners) {
			versioner.ResetToStaged(stagedStatePreview);
		}
	}

	public void HideStagedState() {
		if(stagedStatePreview != null) {
			Destroy(stagedStatePreview);
			Destroy(stagedUIController.gameObject);
			stagedStatePreview = null;
		}
	}

	public GameObject GetStagedObjectPreview() {
		return this.stagedStatePreview;
	}
}
