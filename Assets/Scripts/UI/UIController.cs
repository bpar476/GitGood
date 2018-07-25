using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public GameObject textButttonDialogTemplate;
	
	void Start () {
		
	}
	
	void Update () {
		// Braching
		if (Input.GetKeyDown(KeyCode.Y)) {
			EngineController.Instance().ToggleControls(false);
			GameObject dialog = Instantiate(textButttonDialogTemplate, transform) as GameObject;
			TextButtonDialogController dialogController = dialog.GetComponent<TextButtonDialogController>();
			dialogController.titleText.text = "Checkout Branch";
			dialogController.submitButton.GetComponentInChildren<Text>().text = "Create Branch";
			dialogController.promptText.text = "git checkout -b ";
			dialogController.submitButton.enabled = false;
			dialogController.inputField.onValueChanged.AddListener((s) => {
				Debug.Log(s);
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

			dialogController.inputField.Select();
		}
		// Committing
		else if (Input.GetKeyDown(KeyCode.E)) {
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
				Debug.Log("Commiting staged objects");

				EngineController.Instance().ToggleControls(true);
				Destroy(dialog);
				}
			);

			dialogController.inputField.Select();
		}
	}
}
