using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InMemoryVersionManager : MonoBehaviour, IVersionManager {

	private LinkedList<Vector2> history;
	private Vector2 stagedPosition;

	// Use this for initialization
	private void Awake() {
		history = new LinkedList<Vector2>();
	}

	public void Stage() {
		stagedPosition = new Vector2(transform.position.x, transform.position.y);
	}

	public void Commit(string message) {
		if (history.Count == 0) {
			history.AddFirst(stagedPosition);
		} else {
			history.AddAfter(history.Last, stagedPosition);
		}
	}

	public void ResetToHead(GameObject gobj) {
		transform.position = new Vector3(history.Last.Value.x, history.Last.Value.y, 0);
	}
}
