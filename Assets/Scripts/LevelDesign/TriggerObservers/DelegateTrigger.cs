using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DelegateTrigger : PositiveTriggerObserver {

	public Triggerable delegateTrigger;

	protected abstract bool shouldFire();

	protected override void HandleTrigger(bool state) {
		if (shouldFire()) {
			delegateTrigger.NotifyObservers();
		}
	}

}
