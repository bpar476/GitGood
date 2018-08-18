using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchSelectionMenuButton : MonoBehaviour {

	public Text branchText;

	public void CheckoutSelectedBranch() {
		VersionManager.Instance().Checkout(branchText.text);
	}
}
