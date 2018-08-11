using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagedObjectPreviewController : VersionPreviewController {

	protected override void OnActionButtonClicked() {
		VersionManager.Instance().Unstage(versionedObject);
	}
}
