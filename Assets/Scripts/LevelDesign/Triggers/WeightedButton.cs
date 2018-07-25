using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : Triggerable {

    public bool active = false;

    public Sprite activeSprite;
    public Sprite offSprite;
    public GameObject buttonCap;
    public GameObject buttonBase;

    private int count;

    public BoxCollider2D triggerZone;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.isTrigger) {
            count++;
            UpdateState();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.isTrigger) {
            count--;
            UpdateState();
        }
    }

    private void UpdateState() {
        bool willUpdate = !(active && count > 0);
        active = count > 0;
        if (willUpdate && active) {
            TurnOn();
        } else {
            TurnOff();
        }

    }

    private void TurnOn() {
        buttonBase.GetComponent<SpriteRenderer>().sprite = activeSprite;
        NotifyObservers(true);
    }

    private void TurnOff() {
        NotifyObservers(false);
        buttonBase.GetComponent<SpriteRenderer>().sprite = offSprite;
    }
}
