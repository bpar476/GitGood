using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchMenu : MonoBehaviour {

	public RectTransform branchList;

	private IDictionary<string, GameObject> renderedBranches;

	private void Awake() {
		this.renderedBranches = new Dictionary<string, GameObject>();
	}

	public void Render() {
		foreach(GameObject textObject in renderedBranches.Values) {
			Destroy(textObject);
		}
		renderedBranches.Clear();
		ICollection<string> branches = VersionManager.Instance().GetBranchList();
		Debug.Log(branches.Count);
		float listY = -30f;
		foreach(string branchName in branches) {
			GameObject branchNameTextObject = Instantiate(Resources.Load("UI/SelectableBranch")) as GameObject;
			branchNameTextObject.transform.SetParent(branchList.gameObject.transform);
			RectTransform rectTransform = branchNameTextObject.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(0, listY);
			listY -= 30.0f;

			Text branchNameText = branchNameTextObject.GetComponentInChildren<Text>();
			branchNameText.text = branchName;
			this.renderedBranches.Add(branchName, branchNameTextObject);
		}
	}
}
