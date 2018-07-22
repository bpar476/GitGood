using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTrigger : Triggerable, ITriggerObserver {

	public int numberToTrigger;
	public Collider2D triggerZone;

	public MergeTriggerManager notifier;

    private int triggerCount = 0;
    private bool playerInZone;

	private void Start() {
		notifier.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
        if (state) {
		    if (playerInZone) {
                triggerCount++;
                if (triggerCount == numberToTrigger) {
                    NotifyObservers();
                }
            }
        }
	}

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player")) {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player")) {
            playerInZone = false;
        }
    }

}
