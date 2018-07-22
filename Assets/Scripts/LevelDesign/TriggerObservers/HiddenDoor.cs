using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour, ITriggerObserver {

	public BackgroundSlidingDoor door;

	private bool hidden = true;

	private void Start() {
		door.gameObject.SetActive(false);
	}

	public void HandleTrigger() {
		hidden = false;
		door.gameObject.SetActive(true);
	}
}
