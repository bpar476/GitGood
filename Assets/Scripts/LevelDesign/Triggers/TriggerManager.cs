using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : Triggerable {

    public void Trigger() {
        NotifyObservers();
    }
}
