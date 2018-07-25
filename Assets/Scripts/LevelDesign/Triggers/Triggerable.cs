using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : Condition {

	protected HashSet<ITriggerObserver> observers;

	private void Awake() {
		observers = new HashSet<ITriggerObserver>();
	}

	public void AddObserver(ITriggerObserver observer) {
		observers.Add(observer);
	}

	public void RemoveObserver(ITriggerObserver observer) {
		observers.Remove(observer);
	}

	protected void NotifyObservers() {
		NotifyObservers(true);
	}

	protected void NotifyObservers(bool state) {
		condition = state;
		List<ITriggerObserver> expiredObservers = new List<>();
		foreach (ITriggerObserver observer in observers) {
			observer.HandleTrigger(state);
			if (!observer.enabled) {
				expiredObservers.Add(observer);
			}
		}
		foreach(ITriggerObserver observer in expiredObservers) {
			RemoveObserver(observer);
		}
	}
}
