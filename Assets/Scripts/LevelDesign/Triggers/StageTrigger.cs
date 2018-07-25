using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrigger : Triggerable, ITriggerObserver {

    public TriggerManager notifier;
    public int numberToTrigger;
    public VersionController targetObject;
    public VersionManager versionManager;

    private void Start() {
        notifier.AddObserver(this);
    }

    public void HandleTrigger(bool state) {
        if (state) {
            // Check if object was just staged
        }
    }
}