using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTrigger : DelegateTrigger {

    public string targetBranch = "";
    public VersionController targetObject;
    public VersionManager versionManager;

    protected override bool shouldFire() {
        if (targetObject == null && targetBranch == "") {
            return true;
        } else {
            if (targetBranch == "" || versionManager.GetActiveBranch().GetName() == targetBranch) {
                ICommit commit = versionManager.GetHead();
                if (targetObject == null || commit.ObjectIsTrackedInThisCommit(targetObject) &&
                    commit.ObjectWasChangedInThisCommit(targetObject)) {
                        return true;
                }
            }
        }
        return false;
    } 
}