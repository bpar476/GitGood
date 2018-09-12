using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeUIController : MonoBehaviour {

	public RectTransform mergeStatusPanel;
	public RectTransform unresolvedControllersRegion;
	public Button resolveButton;

	private MergeWorker mw;

	private IDictionary<VersionController, MergeObjectController> controllers;

	private void Awake() {
		this.controllers = new Dictionary<VersionController, MergeObjectController>();
	}

	public void PopulateConflictObjects(ICollection<VersionController> conflictObjects) {
		float yPos = -27.5f;
		foreach (VersionController conflictedObject in conflictObjects) {
			IBranch baseBranch = mw.GetBaseBranch();
			IBranch featureBranch = mw.GetFeatureBranch();

			GameObject uiObj = Instantiate(Resources.Load("UI/MergeUI/UnresolvedObject")) as GameObject;
			MergeObjectController controller = uiObj.GetComponent<MergeObjectController>();

			this.controllers.Add(conflictedObject, controller);

			controller.underlyingObject = conflictedObject;
			controller.baseBranch = baseBranch.GetName();
			controller.SetBasePreview(mw.GetBasePreviewForVersionedObject(conflictedObject));
			controller.featureBranch = featureBranch.GetName();
			controller.SetFeaturePreview(mw.GetFeaturePreviewForVersionedObject(conflictedObject));

			RectTransform rTransform = uiObj.GetComponent<RectTransform>();
			rTransform.SetParent(unresolvedControllersRegion, false);
			rTransform.anchoredPosition = new Vector2(rTransform.anchoredPosition.x, yPos);
			yPos -= 50.0f;
		}
	}

	public void VersionPicked(VersionController versionController, GameObject versionPicked) {
		MergeObjectController uiController;
		if (this.controllers.TryGetValue(versionController, out uiController)) {
			uiController.VersionWasPicked(versionPicked);
			foreach(MergeObjectController controller in controllers.Values) {
				if (controller.IsResolved()) {
					resolveButton.gameObject.SetActive(true);
				}
			}
		}
	}

	public void SetMergeWorker(MergeWorker mergeWorker) {
		this.mw = mergeWorker;
	}

	public void ResolveMerge() {
		UIController.Instance().DisplayMergeCommitDialog();
		controllers.Clear();
	}
}
