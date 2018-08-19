using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSetCommitTriggerBranch : PositiveTriggerObserver {
    public CommitTrigger commitTrigger;
    protected override void HandleTrigger(bool state) {
        string currentBranch = VersionManager.Instance().GetActiveBranch().GetName();
        commitTrigger.targetBranch = currentBranch;
    }
}
