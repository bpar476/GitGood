using UnityEngine;

public class Grabbable : MonoBehaviour {

    public float grabDistance = 0.5f;
    public GameObject grabbedBy;

    public bool Grab(GameObject grabber) {
        if (Vector3.Distance(grabber.transform.position, transform.position) < grabDistance
           && grabbedBy == null) {
            GrabObject grab = grabber.GetComponent<GrabObject>();
            if (grab != null) {
                grab.PickUp(this);
                grabbedBy = grabber;
                return true;
            }
        }
        return false;
    }

    public void Drop() {
        grabbedBy = null;
    }

}