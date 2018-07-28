using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : DelegateTrigger {

    public VersionController targetObject;
    public VersionManager versionManager;

    protected override bool shouldFire() {
        if (versionManager.GetLastStagedObject().Equals(targetObject)) {
            return true;
        }
        return false;
    }
}