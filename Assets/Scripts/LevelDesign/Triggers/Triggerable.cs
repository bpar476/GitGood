using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : Condition {

	protected HashSet<TriggerObserver> observers;

	private void Awake() {
		observers = new HashSet<TriggerObserver>();
	}

	public void AddObserver(TriggerObserver observer) {
		observers.Add(observer);
	}

	public void RemoveObserver(TriggerObserver observer) {
		observers.Remove(observer);
	}

	public void NotifyObservers() {
		NotifyObservers(true);
	}

	public void NotifyObservers(bool state) {
		condition = state;
		foreach (TriggerObserver observer in observers) {
			observer.Triggered(state);
		}
	}
}
