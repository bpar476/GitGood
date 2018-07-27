using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTrigger : Triggerable {

    public List<Condition> conditions;
    public bool oneShot = true;
    public int timesToTrigger = 1;

    private int count = 0;

    private void Update() {
        bool state = true;
        foreach (Condition condition in conditions) {
            state &= condition.getState();
        }
        if (state) {
            count++;
            if (count == timesToTrigger) {
                count = 0;
                NotifyObservers();
                if (oneShot) {
                    this.enabled = false;
                }
            }
        }
    }

}