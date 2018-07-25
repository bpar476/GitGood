using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstageTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public VersionController targetObject;
    public VersionManager versionManager;
    public bool oneShot = true;

    private int count = 0;

    private void Start() {
        notifier.AddObserver(this);
    }

    public void HandleTrigger(bool state) {
        if (state) {
            if (versionManager.GetLastUnstagedObject().Equals(targetObject)) {
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