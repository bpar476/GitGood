using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour, ITriggerObserver {

	public BackgroundSlidingDoor door;
	public Triggerable trigger;

	private bool hidden = true;

	private void Start() {
		trigger.AddObserver(this);
		door.gameObject.SetActive(false);
	}

	public void HandleTrigger(bool state) {
		hidden = !state;
		door.gameObject.SetActive(state);
	}
}
