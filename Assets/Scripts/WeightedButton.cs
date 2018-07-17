using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour {

    public bool active {
        get { return active; }
        private set { active = value; }
    }

    private int count;

    public BoxCollider2D triggerZone;

    private void OnTriggerEnter2D(Collider2D other) {
        count++;
        active = count > 0;
    }

    private void OnTriggerExit2D(Collider2D other) {
        count--;
        active = count > 0;
    }
}
