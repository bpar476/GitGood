using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartTrigger : PositiveTriggerObserver {

	public Flowchart flowchart;
	public string message;

	protected override void HandleTrigger(bool state) {
		flowchart.SendFungusMessage (message);
	}
}
