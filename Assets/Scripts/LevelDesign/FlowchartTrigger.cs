using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartTrigger : MonoBehaviour {

	public Flowchart flowchart;
	public bool oneShot;
	public string message;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals("Player")) {
			flowchart.SendFungusMessage (message);
		}
	}

}
