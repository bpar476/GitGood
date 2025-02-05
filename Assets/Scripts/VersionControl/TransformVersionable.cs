﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVersionable : MonoBehaviour, IVersionable {

	public Transform initialState;

	private float stagedX;
	private float stagedY;
	
	private History<Vector2> history;

	public void Awake() {
		this.history = new History<Vector2>();
		if (this.initialState == null) {
			this.initialState = new GameObject().transform;
		}
	}
	
	public void Stage(GameObject version) {
		this.stagedX = version.transform.position.x;
		this.stagedY = version.transform.position.y;
	}

	public void Commit(IVersion version) {
		history.Add(version, new Vector2(stagedX, stagedY));
	}

	public void ResetToVersion(IVersion version, GameObject target) {
		Reset(target, history.GetStateAt(version));
	}

	public void ResetToStaged(GameObject target) {
		Reset(target, new Vector2(stagedX, stagedY));
	}

	public void ResetToInitialState(GameObject target) {
		Reset(target, this.initialState.position);
	}

	private void Reset(GameObject target, Vector2 state) {
		target.transform.position = state;
		Rigidbody2D rb2d = target.GetComponent<Rigidbody2D>();
		if (rb2d != null) {
			rb2d.velocity = new Vector3(0,0,0);
		}
	}

	public void SetInitialState(Vector2 position) {
		this.initialState.position = new Vector3(position.x, position.y, 0);
	}

	public string DescribeState(IVersion version) {
		Vector2 position = history.GetStateAt(version);
		return "Position: [" + position.x + "," + position.y + "]";
	}

	public string DescribeStagedState() {
		return string.Format("Position: [ {0:F2}, {1:F2}]", stagedX, stagedY);
	}
}
