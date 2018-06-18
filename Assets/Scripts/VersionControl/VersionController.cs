using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<Versionable> versioners;

	public GameObject templatePrefab;
	public Transform initialPosition;

	private GameObject activeVersion;
	private IDictionary<int, GameObject> previewVersions;

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

	public void ResetToCommit(int commitId) {
		foreach (Versionable versioner in versioners) {
			versioner.ResetToCommit(commitId, activeVersion);
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
}
