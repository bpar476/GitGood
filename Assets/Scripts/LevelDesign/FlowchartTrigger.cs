using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartTrigger : MonoBehaviour, ITriggerObserver {

	public Flowchart flowchart;
	public bool oneShot;
	public string message;
	public Trigger trigger;

	private bool triggered;

	public void HandleTrigger(bool state) {
		if (state && !triggered) {
			if (other.tag.Equals("Player")) {
				flowchart.SendFungusMessage (message);
				triggered = true;
			}
		}
	}
}
