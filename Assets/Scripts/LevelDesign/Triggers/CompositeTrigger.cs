using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTrigger : Triggerable {

    public List<Condition> conditions;

    private void Awake() {
        conditions = new List();
    }

    private void Update() {
        bool state = true;
        foreach (Condition condition in conditions) {
            state &= condition.getState();
        }
        if (state) {
            NotifyObservers();
        }
    }

}