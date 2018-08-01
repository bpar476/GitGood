using UnityEngine;

public class GrabObject : MonoBehaviour {

    public Grabbable holding;

    public void PickUp(Grabbable grabee) {
        if (holding == null) {
            TogglePhysics(grabee, false);
            grabee.transform.parent = transform;
            grabee.transform.localPosition = new Vector3(0.75f, 0.2f, 0.0f);
            holding = grabee;
        }
    }

    public void DropHeldObject() {
        TogglePhysics(holding, true);
        holding.transform.parent = null;
        holding.Drop();
        holding = null;
    }

    private void TogglePhysics(Grabbable grabee, bool on) {
        Rigidbody2D rb2d = grabee.GetComponent<Rigidbody2D>();
        if (rb2d != null) {
            rb2d.isKinematic = !on;
        }
        Collider2D collider = grabee.GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger) {
            collider.enabled = on;
        }
    }

}