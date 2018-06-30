using System.Collections.Generic;
using UnityEngine;

public class ClosestObjectVisionObserver : MonoBehaviour, IVisionObserver {

    private IList<GameObject> visibleObjects;

    private void Awake() {
        this.visibleObjects = new List<GameObject>();
    }

    public void ProcessVisibleObject(GameObject gobj) {
    }

    public void ObjectEnteredVisibility(GameObject gobj) {
        visibleObjects.Add(gobj);
    }

    public void ObjectLeftVisibility(GameObject gobj) {
        visibleObjects.Remove(gobj);
    }
    
    public GameObject GetClosestObject() {
        return GetClosestObjectWithTag(null);
    }

    public GameObject GetClosestObjectWithTag(string queryTag) {
        if (this.visibleObjects.Count > 0) {
            GameObject closestObject = null;
            foreach (GameObject candidate in visibleObjects) {
                if (queryTag == null || candidate.tag == queryTag) {
                    if (closestObject == null) {
                        closestObject = candidate;
                    } else if (Vector3.Distance(transform.position, candidate.transform.position) < Vector3.Distance(transform.position, closestObject.transform.position)) {
                        closestObject = candidate;
                    }
                }
                Debug.DrawLine(transform.position, candidate.transform.position, Color.red);
            }
            if (closestObject != null) {
            }
            return closestObject;
        } else {
            return null;
        }
    }

}
