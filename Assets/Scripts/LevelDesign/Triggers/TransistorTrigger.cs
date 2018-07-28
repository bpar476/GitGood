using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistorTrigger : Triggerable, ITriggerObserver {

	public Triggerable collector;
	public Condition baseSignal;

	public bool oneShot = true;
	public int timesToTrigger = 1;

	private int count = 0;

	private void Start() {
		collector.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
		if (baseSignal.getState()) {
			count++;
			if (count == timesToTrigger) {
				count = 0;
				NotifyObservers(state);
				if (oneShot) {
					this.enabled = false;
				}
			}
		}
	}
}
