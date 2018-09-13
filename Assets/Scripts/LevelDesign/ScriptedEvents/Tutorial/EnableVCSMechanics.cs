using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableVCSMechanics : MonoBehaviour {

	public void EnableCommit() {
		EnabledVersionControls.Instance().EnableCommit();
		GameObject.Find("EngineController/UIController/Overlay/Status/CommitButton").GetComponent<Button>().interactable = true;
	}

	public void EnableBranch() {
		EnabledVersionControls.Instance().EnableBranch();
		GameObject.Find("EngineController/UIController/Overlay/Status/BranchButton").GetComponent<Button>().interactable = true;
	}

	public void EnableReset() {
		EnabledVersionControls.Instance().EnableReset();
		GameObject.Find("EngineController/UIController/Overlay/Status/ResetButton").GetComponent<Button>().interactable = true;
	}

	public void EnableMerge() {
		EnabledVersionControls.Instance().EnableMerge();
	}

	public void EnableCheckout() {
		EnabledVersionControls.Instance().EnableCheckout();
		GameObject.Find("EngineController/UIController/Overlay/Status/CheckoutButton").GetComponent<Button>().interactable = true;
	}
}
