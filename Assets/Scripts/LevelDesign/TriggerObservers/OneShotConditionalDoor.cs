using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotConditionalDoor : MonoBehaviour, ITriggerObserver {

	public Triggerable trigger;
	public Animator doorAnimator;
	public Collider2D closeZone;

	private bool hasTriggered;

	// Use this for initialization
	void Start () {
		trigger.AddObserver(this);
		hasTriggered = false;
	}

	public void HandleTrigger(bool state) {
		if (state) {
			if (!hasTriggered) {
				doorAnimator.SetBool("open", true);
			}
			hasTriggered = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals("Player")) {
			doorAnimator.SetBool("open", false);
		}
	}
	

}
