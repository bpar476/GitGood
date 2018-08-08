using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeUIController : MonoBehaviour {

	public RectTransform mergeStatusPanel;
	public RectTransform ResolvedControllersRegion;
	public RectTransform UnresolvedControllersRegion;

	private MergeWorker mw;
	private int numResolved = 0;
	private int numConflicts = 0;

	private IDictionary<VersionController, MergeObjectController> controllers;

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
			rTransform.SetParent(UnresolvedControllersRegion, false);
			rTransform.anchoredPosition = new Vector2(rTransform.anchoredPosition.x, yPos);
			yPos -= 20.0f;
		}
	}

	public void VersionPicked(VersionController versionController, GameObject versionPicked) {
		MergeObjectController uiController;
		if (this.controllers.TryGetValue(versionController, out uiController)) {
			uiController.VersionWasPicked(versionPicked);
		}
	}

	public void SetMergeWorker(MergeWorker mergeWorker) {
		this.mw = mergeWorker;
	}
}
