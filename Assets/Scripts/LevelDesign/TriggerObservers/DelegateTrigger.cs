using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DelegateTrigger : TriggerObserver {

	public Triggerable delegateTrigger;
	public bool oneShot = true;
	public int timesToTrigger = 1;

	private int count = 0;
	private bool triggerAgain = true;

	protected abstract bool shouldFire();

	protected override void HandleTrigger(bool state) {
		if (triggerAgain && shouldFire()) {
			count++;
			if (count == timesToTrigger) {
				count = 0;
				delegateTrigger.NotifyObservers();
				triggerAgain = !oneShot;
			}
		}
	}

}
