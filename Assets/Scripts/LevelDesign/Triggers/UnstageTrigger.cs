using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstageTrigger : DelegateTrigger {

    public VersionController targetObject;
    public VersionManager versionManager;

    protected override bool shouldFire() {
        VersionController lastUnstagedObject = versionManager.GetLastUnstagedObject();
        if (lastUnstagedObject != null && lastUnstagedObject.Equals(targetObject)) {
            return true;
        }
        
        return false;
    }
}