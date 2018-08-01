using UnityEngine;

public class Grabbable : MonoBehaviour {

    public GameObject grabbedBy;

    public void Grab(GameObject grabber) {
        GrabObject grab = grabber.GetCommponent<GrabObject>();
        if (grab != null) {
            grab.PickUp(gameObject);
            grabbedBy = grabber;
        }
    }

}