using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBranchTrigger : Triggerable, ITriggerObserver {

	public TriggerManager notifier;
	public VersionManager VersionManager;
	public int numberToTrigger = 1;
	public bool oneShot = true;

	private int count = 0;

	private void Start() {
        notifier.AddObserver(this);
    }

	public void HandleTrigger(bool state) {
		if (state) {
			count++;
			if (count == numberToTrigger) {
				NotifyObservers();
				count = 0;
				if (oneShot) {
					this.enabled = false;
				}
			}
		}
	}
}
