using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : TriggerObserver {

	public BackgroundSlidingDoor door;

	private bool hidden = true;

	private void Start() {
		door.gameObject.SetActive(false);
	}

	protected override void HandleTrigger(bool state) {
		hidden = !state;
		door.gameObject.SetActive(state);
	}
}
