using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public VersionController targetObject;
    public VersionManager versionManager;

    public void HandleTrigger(bool state) {
        if (state) {
            ICommit commit = versionManager.GetHead();
            if (commit.ObjectIsTrackedInThisCommit(targetObject) &&
                commit.ObjectWasChangedInThisCommit(targetObject)) {
                NotifyObservers();
            }
        }
    }
}