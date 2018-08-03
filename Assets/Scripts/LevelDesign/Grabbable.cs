using UnityEngine;

public class Grabbable : PersistentInteractable {

    public float grabDistance = 1.5f;
    public GameObject grabbedBy;

    private Transform oldParent;

    public override bool TryInteract(GameObject grabber) {
        if (Vector3.Distance(grabber.transform.position, transform.position) < grabDistance
           && grabbedBy == null) {
            TogglePhysics(false);
            oldParent = transform.parent;
            transform.parent = grabber.transform;
            transform.localPosition = new Vector3(0.75f, 0.2f, 0.0f);
            grabbedBy = grabber;
            return true;
        }
        return false;
    }

    public override void StopInteracting() {
        transform.parent = oldParent;
        TogglePhysics(true);
        grabbedBy = null;
    }

    private void TogglePhysics(bool on) {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null) {
            rb2d.isKinematic = !on;
        }
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger) {
            collider.enabled = on;
        }
    }
}
