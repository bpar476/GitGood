using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneTrigger : Triggerable {

	public Collider2D triggerZone;
	public bool oneShot = true;
	public int timesToTrigger = 1;

	private int count;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals("Player")) {
			count++;
			if (count == timesToTrigger) {
				this.NotifyObservers();
				if (oneShot) {
					this.enabled = false;
				}
			}
		}
	}
	
}
