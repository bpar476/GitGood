using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeTriggerManager : Triggerable {

    public void Trigger() {
        NotifyObservers();
    }
}
