using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTrigger : Triggerable, ITriggerObserver {

	public int numberToTrigger;
	public Collider2D triggerZone;

	public TriggerManager notifier;

    private int triggerCount = 0;
    private bool playerInZone;
    public bool oneShot;

	private void Start() {
		notifier.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
        Debug.Log("Merge trigger triggered");
        if (state) {
		    if (playerInZone) {
                triggerCount++;
                if (triggerCount == numberToTrigger) {
                    NotifyObservers();
                    triggerCount = 0;
                    if (oneShot) {
                        this.enabled = false;
                    }
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
