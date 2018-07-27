using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public string targetBranch = "";
    public VersionController targetObject;
    public VersionManager versionManager;
    public bool oneShot = true;

    private int count = 0;
    private bool triggered = false;

    private void Start() {
        notifier.AddObserver(this);
    }

    public void HandleTrigger(bool state) {
        if (!triggered && state) {
            if (targetObject == null && targetBranch == "") {
                NotifyObservers();
            } else {
                if (targetBranch == "" || versionManager.GetActiveBranch().GetName() == targetBranch) {
                    ICommit commit = versionManager.GetHead();
                    if (targetObject == null || commit.ObjectIsTrackedInThisCommit(targetObject) &&
                        commit.ObjectWasChangedInThisCommit(targetObject)) {
                            count++;
                            if (count == numberToTrigger) {
                                NotifyObservers();
                                count = 0;
                                if (oneShot) {
                                    this.enabled = false;
                                }
                            }
                    }
                }
            }
        }
    }
}