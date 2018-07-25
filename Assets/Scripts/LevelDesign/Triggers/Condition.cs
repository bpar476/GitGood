using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : MonoBehaviour {

	protected bool condition = false;

	public bool getState() {
		return condition;
	}
}
