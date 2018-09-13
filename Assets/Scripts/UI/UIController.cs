using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	public GameObject textButttonDialogTemplate;

	public BranchMenu branchMenu;

	public Triggerable checkoutDialogTrigger;
	public Triggerable newBranchDialogTrigger;
	public Triggerable commitDialogTrigger;
	public Triggerable mergeDialogTrigger;

	private UIController() {

    }

	void Start() {
		EnabledVersionControls playerVersionControls = EnabledVersionControls.Instance();
		GameObject.Find("Overlay/Status/CommitButton").GetComponent<Button>().interactable = playerVersionControls.CanCommit;
		GameObject.Find("Overlay/Status/BranchButton").GetComponent<Button>().interactable = playerVersionControls.CanBranch;
		GameObject.Find("Overlay/Status/CheckoutButton").GetComponent<Button>().interactable = playerVersionControls.CanCheckout;
		GameObject.Find("Overlay/Status/ResetButton").GetComponent<Button>().interactable = playerVersionControls.CanReset;
		UpdateOverlay();
	}

	void Update () {
		// Branching
		if (Input.GetKeyDown(KeyCode.Y)) {
			DisplayBranchDialog();
		}
		// Committing
		else if (Input.GetKeyDown(KeyCode.E)) {
			DisplayCommitDialog();
		}
		// Merging
		else if (Input.GetKeyDown(KeyCode.M)) {
			if (VersionManager.Instance().GetMergeWorker() != null) {
				if (!VersionManager.Instance().GetMergeWorker().IsResolved()) {
					Debug.Log("Merge not resolved");
					return;
				}
				DisplayMergeCommitDialog();
			}
			else {
				DisplayMergeDialog();
			}
		}
	}

	public void DisplayChooseBranchDialog() {
		EngineController.Instance().ToggleControls(false);
		branchMenu.gameObject.SetActive(true);
		branchMenu.Render();
		if (checkoutDialogTrigger != null) {
			checkoutDialogTrigger.NotifyObservers();
		}
	}

	public void DisplayBranchDialog() {
		if (!EnabledVersionControls.Instance().CanBranch) {
			Debug.Log("Can't Branch; not enabled yet");
			return;
		}
		EngineController.Instance().ToggleControls(false);
		GameObject dialog = Instantiate(textButttonDialogTemplate, transform) as GameObject;
		TextButtonDialogController dialogController = dialog.GetComponent<TextButtonDialogController>();
		dialogController.titleText.text = "Checkout Branch";
		dialogController.submitButton.GetComponentInChildren<Text>().text = "Create Branch";
		dialogController.promptText.text = "git checkout -b ";
		dialogController.submitButton.enabled = false;
		dialogController.inputField.onValueChanged.AddListener((s) => {
			if (VersionManager.Instance().HasBranch(s)) {
				dialogController.submitButton.GetComponentInChildren<Text>().text = "Switch to Branch";
				dialogController.promptText.text = "git checkout ";
			}
			else {
				dialogController.submitButton.GetComponentInChildren<Text>().text = "Create Branch";
				dialogController.promptText.text = "git checkout -b ";
			}
			dialogController.submitButton.enabled = !s.Equals("");
		});
		dialogController.submitButton.onClick.AddListener(() => {
			Debug.Log("Button pressed");

			string branch = dialogController.inputField.text;
			if (Regex.Match(branch, @"[~^:\[\]\\\s]").Success) {
				dialogController.ErrorText = "Error - branch names cannot contain spaces or special characters: ~ ^ : [ ] \\";
				return;
			}

			if (!VersionManager.Instance().HasBranch(branch)) {
				VersionManager.Instance().CreateBranch(branch);
				Debug.Log("Creating branch " + branch);
			}
			VersionManager.Instance().Checkout(branch);
			Debug.Log("Checkout " + branch);

			EngineController.Instance().ToggleControls(true);
			Destroy(dialog);
			}
		);

		dialogController.submitButton.onClick.AddListener(() => {
			UpdateOverlay();
		});

		dialogController.inputField.Select();

		if (newBranchDialogTrigger != null) {
			newBranchDialogTrigger.NotifyObservers();
		}
	}

	public void DisplayCommitDialog() {
		if (!EnabledVersionControls.Instance().CanCommit) {
			Debug.Log("Can't commit; not enabled");
			return;
		}
		EngineController.Instance().ToggleControls(false);
		GameObject dialog = Instantiate(textButttonDialogTemplate, transform) as GameObject;
		TextButtonDialogController dialogController = dialog.GetComponent<TextButtonDialogController>();
		dialogController.promptText.text = "git commit -m ";
		dialogController.titleText.text = "Enter a commit message";
		dialogController.submitButton.enabled = false;
		dialogController.submitButton.GetComponentInChildren<Text>().text = "Commit";
		dialogController.inputField.onValueChanged.AddListener((s) => {
			dialogController.submitButton.enabled = !s.Equals("");
		});
		dialogController.submitButton.onClick.AddListener(() => {
			Debug.Log("Button pressed");

			string commitMessage = dialogController.inputField.text;
			VersionManager.Instance().Commit(commitMessage);
			Debug.Log("Commiting");

			EngineController.Instance().ToggleControls(true);
			Destroy(dialog);
			}
		);

		dialogController.submitButton.onClick.AddListener(() => {
			UpdateOverlay();
		});

		dialogController.inputField.Select();

		if (commitDialogTrigger != null) {
			commitDialogTrigger.NotifyObservers();
		}
	}

	public void DisplayMergeCommitDialog() {
		EngineController.Instance().ToggleControls(false);
		GameObject dialog = Instantiate(textButttonDialogTemplate, transform) as GameObject;
		TextButtonDialogController dialogController = dialog.GetComponent<TextButtonDialogController>();
		dialogController.promptText.text = "git commit -m ";
		dialogController.titleText.text = "Enter a merge commit message";
		dialogController.submitButton.enabled = false;
		dialogController.submitButton.GetComponentInChildren<Text>().text = "Commit";
		dialogController.inputField.onValueChanged.AddListener((s) => {
			dialogController.submitButton.enabled = !s.Equals("");
		});
		dialogController.submitButton.onClick.AddListener(() => {
			Debug.Log("Button pressed");

			string commitMessage = dialogController.inputField.text;
			VersionManager.Instance().CreateMergeCommit(commitMessage);
			Debug.Log("Proceeding with merge commit");

			EngineController.Instance().ToggleControls(true);
			Destroy(dialog);

			Camera.main.GetComponent<MergeInterfaceCamera>().enabled = false;
			}
		);

		dialogController.submitButton.onClick.AddListener(() => {
			UpdateOverlay();
		});

		dialogController.inputField.Select();

		if (commitDialogTrigger != null) {
			commitDialogTrigger.NotifyObservers();
		}
	}

	public void DisplayMergeDialog() {
		if (!EnabledVersionControls.Instance().CanMerge) {
			Debug.Log("Can't Merge; not enabled yet");
			return;
		}
		EngineController.Instance().ToggleControls(false);
		GameObject dialog = Instantiate(textButttonDialogTemplate, transform) as GameObject;
		TextButtonDialogController dialogController = dialog.GetComponent<TextButtonDialogController>();
		dialogController.promptText.text = "git merge ";
		dialogController.titleText.text = "Enter Feature Branch";
		dialogController.submitButton.enabled = false;
		dialogController.submitButton.GetComponentInChildren<Text>().text = "Merge";
		dialogController.inputField.onValueChanged.AddListener((s) => {
			dialogController.submitButton.enabled = !s.Equals("");
		});
		dialogController.submitButton.onClick.AddListener(() => {
			bool reenableControls = true;
			Debug.Log("Button pressed");

			string mergeBranch = dialogController.inputField.text;
			if (VersionManager.Instance().HasBranch(mergeBranch)) {
				VersionManager.Instance().Merge(VersionManager.Instance().LookupBranch(mergeBranch));
				if (VersionManager.Instance().IsInMergeConflict()) {
					// The merge conflict menu will re-enable controls later.
					reenableControls = false;
				}
				Debug.Log("Starting merge");
			}
			else {
				dialogController.ErrorText = "Feature branch doesn't exist, check spelling and try again";
				return;
			}

			if (reenableControls) {
				EngineController.Instance().ToggleControls(true);
			}
			Destroy(dialog);
			}
		);

		dialogController.submitButton.onClick.AddListener(() => {
			UpdateOverlay();
		});

		dialogController.inputField.Select();

		if (mergeDialogTrigger != null) {
			mergeDialogTrigger.NotifyObservers();
		}
	}

	public void UpdateOverlay() {
		if (GameObject.Find("Overlay/Status") != null) {
			Text branchText = GameObject.Find("Overlay/Status/CurrentBranch").GetComponent<Text>();
			if (VersionManager.Instance().GetActiveBranch() != null) {
				branchText.text = "Current Branch: " + VersionManager.Instance().GetActiveBranch().GetName();
			}
			else {
				branchText.text = "Current Branch: ERROR";
			}

			Text commitText = GameObject.Find("Overlay/Status/CommitMessage").GetComponent<Text>();
			if (VersionManager.Instance().GetHead() != null) {
				commitText.text = "Last Commit: " + VersionManager.Instance().GetHead().GetMessage();
			}
			else {
				commitText.text = "Last Commit: {No Commit}";
			}

			List<String> objectNames = VersionManager.Instance().GetStagedObjectNames();
			Text stagedAreaText = GameObject.Find("/EngineController/UIController/Overlay/Status/StagingArea").GetComponent<Text>();

			if (objectNames.Count == 0) {
				stagedAreaText.text = "Nothing staged";
				stagedAreaText.color = Color.grey;
			}
			else {
				stagedAreaText.text = String.Join("\n", objectNames.ToArray());
				stagedAreaText.color = Color.black;
			}
		}
	}
}
