using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistorTrigger : DelegateTrigger {

	public Condition baseSignal;

	protected override bool shouldFire() {
		return baseSignal.getState();
	}
}
