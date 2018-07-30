using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutTrigger : DelegateTrigger {

	public VersionManager versionManager;
	public string targetBranch = "";

	protected override bool shouldFire() {
		return targetBranch == "" || versionManager.GetActiveBranch().GetName() == targetBranch;
	}
}
