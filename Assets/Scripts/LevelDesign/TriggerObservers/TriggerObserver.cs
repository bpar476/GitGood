using UnityEngine;

public abstract class TriggerObserver : MonoBehaviour {

	public Triggerable trigger;

	private void Start() {
		trigger.AddObserver(this);
	}

	public virtual void Triggered(bool state) {
		HandleTrigger(state);
	}

	protected abstract void HandleTrigger(bool state);
}
