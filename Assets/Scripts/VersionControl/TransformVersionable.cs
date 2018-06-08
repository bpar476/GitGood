using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVersionable : MonoBehaviour, Versionable {

	private float stagedX;
	private float stagedY;

	private History<Vector2> history;

	private void Awake() {
		history = new History<Vector2>();
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Stage() {
		stagedX = transform.position.x;
		stagedY = transform.position.y;
	}

	public void Commit(int commitId) {
		history.Add(commitId, new Vector2(stagedX, stagedY));
	}

	public void ResetToCommit(int commitId) {
		this.transform.position = history.Load(commitId);
	}
}
