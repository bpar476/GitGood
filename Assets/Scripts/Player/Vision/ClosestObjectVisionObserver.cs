using System.Collections.Generic;
using UnityEngine;

public class ClosestObjectVisionObserver : MonoBehaviour, IVisionObserver {

    public string queryTag = "Untagged";
    public bool useTag = false;

    private GameObject closestObject;

    public void ProcessVisibleObject(GameObject gobj) {
        if (!useTag || gobj.tag == this.queryTag) {
            if (this.closestObject == null) {
                this.closestObject = gobj;
            } else if (Vector3.Distance(transform.position, gobj.transform.position) < Vector3.Distance(transform.position, this.closestObject.transform.position)) {
                this.closestObject = gobj;
            }
        }
    }

    public void ObjectEnteredVisibility(GameObject gobj) {

    }

    public void ObjectLeftVisibility(GameObject gobj) {
        if (this.closestObject != null && this.closestObject.Equals(gobj)) {
            this.closestObject = null;
        }
    }

    public GameObject getClosestObject() {
        return this.closestObject;
    }
}
