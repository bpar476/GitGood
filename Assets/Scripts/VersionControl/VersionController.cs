using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<Versionable> versioners;

	public GameObject templatePrefab;
	public GameObject previewPrefab;
	public Transform initialPosition;

	private GameObject activeVersion;
	private IDictionary<int, GameObject> previewVersions;
	private GameObject stagedStatePreview;

	public bool transformVersionable;
	private int version = 0;

	private void Awake() {
		versioners = new List<Versionable>();	
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
	
	public void Stage() {
		foreach (Versionable versioner in versioners) {
			versioner.Stage(activeVersion);
		}
	}

	public int GenerateVersion() {
		this.version++;
		foreach (Versionable versioner in versioners) {
			versioner.Commit(version);
		}
		return version;
	}

	public int GetVersion() {
		return this.version;
	}

	public void ResetToVersion(int version) {
		foreach (Versionable versioner in versioners) {
			versioner.ResetToVersion(version, activeVersion);
		}
	}

	public void AddVersionable(Versionable versioner) {
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

	public void ShowCommit(int commitId) {
		GameObject preview;
		if(!previewVersions.TryGetValue(commitId, out preview)) {
			preview = Instantiate(previewPrefab, transform) as GameObject;
			foreach (Versionable versioner in versioners) {
				versioner.ResetToVersion(commitId, preview);
			}
			SpriteRenderer renderer = preview.GetComponent<SpriteRenderer>();
			renderer.color = new Color(0.5f, 0.1f, 0.0f, 0.4f);
			previewVersions.Add(commitId, preview);
		}
	}

	public void ShowStagedState() {
		if(stagedStatePreview == null) {
			stagedStatePreview =  Instantiate(previewPrefab, transform) as GameObject;
		}
		foreach (Versionable versioner in versioners) {
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
