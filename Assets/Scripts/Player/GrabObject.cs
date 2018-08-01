using UnityEngine;

public class GrabObject : MonoBehaviour {

    public Grabbable holding;

    public void PickUp(Grabbable grabee) {
        if (holding == null) {
            grabee.transform.parent = transform;
            grabee.transform.localPosition = new Vector3(0.75f, 0.2f, 0.0f);
            holding = grabee;
        }
    }

    public void DropHeldObject() {
        holding.transform.parent = null;
        holding.Drop();
        holding = null;
    }

}