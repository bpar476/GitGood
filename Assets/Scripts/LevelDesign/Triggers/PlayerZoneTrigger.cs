using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneTrigger : Triggerable {

	public Collider2D triggerZone;
	public bool oneShot = true;
	public int timesToTrigger = 1;

	private int count;
	private bool triggered = false;

	private void OnTriggerEnter2D(Collider2D other) {
		if (!triggered && other.gameObject.tag.Equals("Player")) {
			count++;
			if (count == timesToTrigger) {
				this.NotifyObservers();
				count = 0;
				if (oneShot) {
					triggered = true;
				}
			}
		}
	}
	
}
