using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVersionable : Versionable {

	private float stagedX;
	private float stagedY;
	
	private History<Vector2> history;

	public TransformVersionable(GameObject gobj) {
		history = new History<Vector2>();
	}

	// Use this for initialization
	void Start () {
		
	}

	public void Stage(GameObject version) {
		stagedX = version.transform.position.x;
		stagedY = version.transform.position.y;
	}

	public void Commit(int commitId) {
		history.Add(commitId, new Vector2(stagedX, stagedY));
	}

	public void ResetToCommit(int commitId, GameObject target) {
		target.transform.position = history.Load(commitId);
	}

	public void ResetToStaged(GameObject target) {
		target.transform.position = new Vector2(stagedX, stagedY);
	}
}
