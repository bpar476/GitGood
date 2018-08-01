using UnityEngine;

public class Grabbable : MonoBehaviour {

    public GameObject grabbedBy;

    public void Grab(GameObject grabber) {
        if (grabbedBy == null) {
            GrabObject grab = grabber.GetCommponent<GrabObject>();
            if (grab != null) {
                grab.PickUp(this);
                grabbedBy = grabber;
            }
        }
    }

    public void Drop() {
        grabbedBy = null;
    }

}