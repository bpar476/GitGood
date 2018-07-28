using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Not actually forced to be oneshot, need to refactor
public class OneShotConditionalDoor : PositiveTriggerObserver {

	public Animator doorAnimator;
	public Collider2D closeZone;

	protected override void HandleTrigger(bool state) {
		doorAnimator.SetBool("open", true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals("Player")) {
			doorAnimator.SetBool("open", false);
		}
	}
	

}
