using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public VersionController targetObject;
    public VersionManager versionManager;

    private void Start() {
        notifier.AddObserver(this);
    }

    public void HandleTrigger(bool state) {
        if (state) {
            if (targetObject == null) {
                NotifyObservers();
            } else {
                ICommit commit = versionManager.GetHead();
                if (commit.ObjectIsTrackedInThisCommit(targetObject) &&
                    commit.ObjectWasChangedInThisCommit(targetObject)) {
                        NotifyObservers();
                }
            }
        }
    }
}