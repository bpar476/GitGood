using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<IVersionable> versioners;

	public GameObject templatePrefab;
	public GameObject previewPrefab;
	public Transform initialPosition;

	private GameObject activeVersion;
	private IDictionary<int, GameObject> previewVersions;
	private GameObject stagedStatePreview;

	public bool transformVersionable;
	private int version = 0;

	private void Awake() {
		versioners = new List<IVersionable>();
		if (transformVersionable) {
			versioners.Add(new TransformVersionable(gameObject));
		}
	}

	// Use this for initialization
	void Start () {
		if (activeVersion == null) {
			activeVersion = Instantiate(templatePrefab, transform) as GameObject;
			activeVersion.transform.position = initialPosition.position;
		}
	}
	
	public void StageVersion() {
		foreach (IVersionable versioner in versioners) {
			versioner.Stage(activeVersion);
		}
	}

	public int GenerateVersion() {
		this.version++;
		foreach (IVersionable versioner in versioners) {
			versioner.Commit(version);
		}
		return version;
	}

	public int GetVersion() {
		return this.version;
	}

	public void ResetToVersion(int version) {
		foreach (IVersionable versioner in versioners) {
			versioner.ResetToVersion(version, activeVersion);
		}
	}

	public void AddVersionable(IVersionable versioner) {
		if (versioner != null) {
			versioners.Add(versioner);
		}
	}

	public void SetActiveVersion(GameObject gobj) {
		activeVersion = gobj;
	}

	public GameObject GetActiveVersion() {
		return activeVersion;
	}

	public void ShowVersion(int version) {
		GameObject preview;
		if(!previewVersions.TryGetValue(version, out preview)) {
			preview = Instantiate(previewPrefab, transform) as GameObject;
			foreach (IVersionable versioner in versioners) {
				versioner.ResetToVersion(version, preview);
			}
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
