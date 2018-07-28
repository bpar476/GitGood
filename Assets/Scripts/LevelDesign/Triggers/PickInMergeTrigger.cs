using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickInMergeTrigger : DelegateTrigger {

	public VersionController targetObject;
	public bool intoMaster;

	protected override bool shouldFire() {
		return true;
	}
}
