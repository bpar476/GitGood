using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVersionable : Versionable {

	private float stagedX;
	private float stagedY;
	
	private GameObject gameObject;

	private History<Vector2> history;

	public TransformVersionable(GameObject gobj) {
		gameObject = gobj;
		history = new History<Vector2>();
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Stage() {
		stagedX = gameObject.transform.position.x;
		stagedY = gameObject.transform.position.y;
	}

	public void Commit(int commitId) {
		history.Add(commitId, new Vector2(stagedX, stagedY));
	}

	public void ResetToCommit(int commitId) {
		gameObject.transform.position = history.Load(commitId);
	}
}
