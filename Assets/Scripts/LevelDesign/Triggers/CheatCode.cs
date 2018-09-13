using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : Triggerable {

	public List<string> sequence;

	private int upTo = 0;
	private bool on = false;

	// Update is called once per frame
	void Update () {
		if (this.upTo == this.sequence.Count) {
			this.on = !this.on;
			this.NotifyObservers(this.on);
			this.upTo = 0;
			Debug.Log("Cheat sequence completed! " + this.on);
		} else {
			KeyCode kc = (KeyCode) System.Enum.Parse(typeof(KeyCode), sequence[upTo]);
			if (Input.anyKeyDown) {
				if (Input.GetKeyDown(kc)) {
					// The player has pressed the key in the correct sequence
					Debug.Log(sequence[upTo] + " has been pressed");
					this.upTo += 1;
				} else if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ||
					Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3) || Input.GetMouseButtonDown(4) ||
					Input.GetMouseButtonDown(5) || Input.GetMouseButtonDown(6))) {

					// The player has pressed a keyboard key not in the sequence
					Debug.Log("Cheat sequence broken");
					this.upTo = 0;
				}
			}
		}
	}
}
