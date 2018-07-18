using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneTrigger : Triggerable {

	public Collider2D triggerZone;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals("Player")) {
			this.NotifyObservers();
		}
	}
	
}
