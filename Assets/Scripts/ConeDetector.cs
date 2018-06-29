using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour {

	public bool debug = false;
	public float fovAngle = 65.0f;
	public CircleCollider2D triggerZone;
	public Transform forwards;

	private ICollection<GameObject> visibleObjects;
	private ICollection<IVisionObserver> observers;

	private void Awake() {
		visibleObjects = new HashSet<GameObject>();
		observers = new HashSet<IVisionObserver>();
	}

	private void Update() {
		if(debug && triggerZone != null) {
			Vector3 forwardDirection = forwards.position - transform.position;
			Vector3 positiveRotatedVector = Quaternion.AngleAxis(fovAngle, Vector3.forward) * forwardDirection;
			Vector3 negativeRotatedVector = Quaternion.AngleAxis(360 - fovAngle, Vector3.forward) * forwardDirection;
			Debug.DrawRay(transform.position, positiveRotatedVector.normalized * triggerZone.radius, Color.blue);
			Debug.DrawRay(transform.position, negativeRotatedVector.normalized * triggerZone.radius, Color.blue);

			if (visibleObjects.Count != 0) {
				foreach(GameObject gobj in visibleObjects) {
					Debug.DrawLine(transform.position, gobj.transform.position, Color.green);
				}
			}
		}	
	}

	// Is within radius
	private void OnTriggerStay2D(Collider2D other) {
		Vector3 otherPosition = other.transform.position;
		Vector3 myPosition = transform.position;
		Vector3 forwardDirection = forwards.position - myPosition;
		Vector3 directionOfObject = otherPosition - myPosition;

		float angle = Vector2.Angle(forwardDirection, directionOfObject);
		if (angle < fovAngle) {
			RaycastHit2D raycastHit = Physics2D.Linecast(transform.position, other.transform.position);
			if (raycastHit.collider != null && raycastHit.collider.Equals(other)) {
				if (debug) {
					Debug.DrawLine(myPosition, otherPosition, Color.red);
				}
				visibleObjects.Add(other.gameObject);
			} else if (visibleObjects.Contains(other.gameObject)) {
				visibleObjects.Remove(other.gameObject);
			}
		} else if (visibleObjects.Contains(other.gameObject)) {
			visibleObjects.Remove(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (visibleObjects.Contains(other.gameObject)) {
			visibleObjects.Remove(other.gameObject);
		}
	}
}
