using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour {

    private bool _active;
    public bool active {
        get { return _active; }
        private set { 
            if (_active != value) {
                if (_active) {
                    buttonBase.GetComponent<SpriteRenderer>().sprite = offSprite;                                      
                } else {
                    buttonBase.GetComponent<SpriteRenderer>().sprite = activeSprite;
                }
            }
            _active = value; 
        }
    }

    public Sprite activeSprite;
    public Sprite offSprite;
    public GameObject buttonCap;
    public GameObject buttonBase;

    private int count;

    public BoxCollider2D triggerZone;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.isTrigger) {
            count++;
            active = count > 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.isTrigger) {
            count--;
            active = count > 0;
        }
    }
}
