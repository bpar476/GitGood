using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour {

	protected bool state = false;

	public bool getState() {
		return state;
	}
}
