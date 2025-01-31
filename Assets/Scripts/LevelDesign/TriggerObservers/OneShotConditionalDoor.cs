﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Not actually forced to be oneshot, need to refactor
public class OneShotConditionalDoor : PositiveTriggerObserver {

	public Animator doorAnimator;
	public Collider2D closeZone;

	private bool shut = false;

	protected override void HandleTrigger(bool state) {
		doorAnimator.SetBool("open", true);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!shut) {
			if (other.gameObject.tag.Equals("Player")) {
				doorAnimator.SetBool("open", false);
				// Commit the player's position because the door can't open again.
				VersionController playerVersionController = other.gameObject.GetComponentInParent<VersionController>();
				VersionManager manager = VersionManager.Instance();
				manager.Add(playerVersionController);
				manager.Commit("Left room", true);
				UIController.Instance().UpdateOverlay();
				GameObject.Find("AutocommitText").GetComponent<FlashText>().Flash();
				shut = true;
			}
		}
	}
}
