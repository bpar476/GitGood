using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutTrigger : Triggerable, ITriggerObserver {

	public TriggerManager notifier;
	public VersionManager versionManager;
	public bool oneShot = true;
	public int numberToTrigger = 1;
	public string targetBranch = "";

	private int count = 0;

	private void Start() {
		notifier.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
		if (targetBranch == "" || versionManager.GetActiveBranch().GetName() == targetBranch) {
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
