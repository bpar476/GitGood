using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVersionable : IVersionable {

	private float stagedX;
	private float stagedY;
	
	private History<Vector2> history;

	public TransformVersionable(GameObject gobj) {
		history = new History<Vector2>();
	}
	
	public void Stage(GameObject version) {
		stagedX = version.transform.position.x;
		stagedY = version.transform.position.y;
	}

	public void Commit(int version) {
		history.Add(version, new Vector2(stagedX, stagedY));
	}

	public void ResetToVersion(int version, GameObject target) {
		Reset(target, history.GetStateAt(version));
	}

	public void ResetToStaged(GameObject target) {
		Reset(target, new Vector2(stagedX, stagedY));
	}

	private void Reset(GameObject target, Vector2 state) {
		target.transform.position = state;
		Rigidbody2D rb2d = target.GetComponent<Rigidbody2D>();
		if (rb2d != null) {
			rb2d.velocity = new Vector3(0,0,0);
		}
	}
}
