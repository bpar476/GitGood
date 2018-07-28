using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PositiveTriggerObserver : TriggerObserver {

	public bool oneShot;
	public int timesToTrigger;

	private int count = 0;
	private bool triggerAgain = true;

	public override void Triggered(bool state) {
		if (state && triggerAgain) {
			count++;
			if (count == timesToTrigger) {
				count = 0;
				HandleTrigger(state);
				if (oneShot) {
					triggerAgain = false;
				}
			}
		}
	}
}
