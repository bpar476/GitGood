using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartTrigger : MonoBehaviour, ITriggerObserver {

	public Flowchart flowchart;
	public string message;
	public Triggerable trigger;

	private bool triggered;

	private void Start() {
		trigger.AddObserver(this);
	}

	public void HandleTrigger(bool state) {
		if (state) {
			flowchart.SendFungusMessage (message);
		}
	}
}
