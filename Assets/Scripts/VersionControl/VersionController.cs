using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour {

	private List<Versionable> versioners;

	public bool transformVersionable;

	private void Awake() {
		versioners = new List<Versionable>();	
		if (transformVersionable) {
			versioners.Add(new TransformVersionable(gameObject));
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	public void Stage() {
		foreach (Versionable versioner in versioners) {
			versioner.Stage();
		}
	}

	public void Commit(int commitId) {
		foreach (Versionable versioner in versioners) {
			versioner.Commit(commitId);
		}
	} 

	public void ResetToCommit(int commitId) {
		foreach (Versionable versioner in versioners) {
			versioner.ResetToCommit(commitId);
		}
	}

	public void AddVersionable(Versionable versioner) {
		if (versioner != null) {
			versioners.Add(versioner);
		}
	}
}
