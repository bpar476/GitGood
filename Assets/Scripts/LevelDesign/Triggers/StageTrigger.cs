using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : DelegateTrigger {

    public VersionController targetObject;
    public VersionManager versionManager;

    protected override bool shouldFire() {
        Debug.Log("In stage trigger shouldFire");
        if (versionManager.GetLastStagedObject().Equals(targetObject)) {
            Debug.Log("Stage trigger should fire");
            return true;
        }
        return false;
    }
}