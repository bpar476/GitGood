using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	public GameObject textButttonDialogTemplate;

	private UIController() {

    }

	void Start() {
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

	public void DisplayBranchDialog() {
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
	}

	public void DisplayCommitDialog() {
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
	}

	public void DisplayMergeDialog() {
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
			Debug.Log("Button pressed");

			string mergeBranch = dialogController.inputField.text;
			if (VersionManager.Instance().HasBranch(mergeBranch)) {
				VersionManager.Instance().Merge(VersionManager.Instance().LookupBranch(mergeBranch));
				Debug.Log("Starting merge");
			}
			else {
				Debug.Log("Feature branch doesn't exist");
			}

			EngineController.Instance().ToggleControls(true);
			Destroy(dialog);
			}
		);

		dialogController.submitButton.onClick.AddListener(() => {
			UpdateOverlay();
		});

		dialogController.inputField.Select();
	}

	private void UpdateOverlay() {
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
	}
}
