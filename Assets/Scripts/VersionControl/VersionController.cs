using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<IVersionable> versioners;

	public GameObject templatePrefab;
	public GameObject previewPrefab;
	public Transform initialPosition;
	public TransformVersionable transformVersioner;

	private GameObject activeVersion;
	private IDictionary<IVersion, GameObject> previewVersions;
	private GameObject stagedStatePreview;

	public bool transformVersionable;
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
	
	public void StageVersion() {
		foreach (IVersionable versioner in versioners) {
			versioner.Stage(activeVersion);
		}
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

	public void ShowStagedState() {
		if(stagedStatePreview == null) {
			stagedStatePreview =  Instantiate(previewPrefab, transform) as GameObject;
		}
		foreach (IVersionable versioner in versioners) {
			versioner.ResetToStaged(stagedStatePreview);
		}
	}

	public void HideStagedState() {
		if(stagedStatePreview != null) {
			Destroy(stagedStatePreview);
			stagedStatePreview = null;
		}
	}
}
