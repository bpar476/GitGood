using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public string targetBranch = "";
    public VersionController targetObject;
    public VersionManager versionManager;

    private void Start() {
        notifier.AddObserver(this);
    }

    public void HandleTrigger(bool state) {
        if (state) {
            if (targetObject == null && targetBranch == "") {
                NotifyObservers();
            } else {
                if (targetBranch == "" || versionManager.GetActiveBranch().GetName() == targetBranch) {
                    ICommit commit = versionManager.GetHead();
                    if (targetObject == null || commit.ObjectIsTrackedInThisCommit(targetObject) &&
                        commit.ObjectWasChangedInThisCommit(targetObject)) {
                            NotifyObservers();
                    }
                }
            }
        }
    }
}