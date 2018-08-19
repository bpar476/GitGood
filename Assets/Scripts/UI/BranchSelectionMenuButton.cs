using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchSelectionMenuButton : MonoBehaviour {

	public Text branchText;
	public BranchMenu branchMenu;

	public void CheckoutSelectedBranch() {
		VersionManager.Instance().Checkout(branchText.text);
		GameObject.Find("BranchesPanel").SetActive(false);
		EngineController.Instance().ToggleControls(true);
	}
}
