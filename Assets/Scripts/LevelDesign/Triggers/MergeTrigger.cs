using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTrigger : Triggerable, ITriggerObserver {

	public int numberToTrigger = 1;
    public bool oneShot = true;

	public TriggerManager notifier;

    private int triggerCount = 0;

	private void Start() {
		notifier.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
        Debug.Log("Merge trigger triggered");
        if (state) {
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
