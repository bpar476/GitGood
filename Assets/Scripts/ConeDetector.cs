using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour {

	public bool debug = false;
	public float fovAngle = 65.0f;
	public string tagToDetect = "";
	public CircleCollider2D triggerZone;
	public Transform forwards;

	private GameObject closestObject;

	private void Update() {
		if(debug && triggerZone != null) {
			Vector3 forwardDirection = forwards.position - transform.position;
			Vector3 positiveRotatedVector = Quaternion.AngleAxis(fovAngle, Vector3.forward) * forwardDirection;
			Vector3 negativeRotatedVector = Quaternion.AngleAxis(360 - fovAngle, Vector3.forward) * forwardDirection;
			Debug.DrawRay(transform.position, positiveRotatedVector.normalized * triggerZone.radius, Color.blue);
			Debug.DrawRay(transform.position, negativeRotatedVector.normalized * triggerZone.radius, Color.blue);

			if (closestObject != null) {
				Debug.DrawLine(transform.position, closestObject.transform.position, Color.green);
			}
		}	
	}
	// Is within radius
	private void OnTriggerStay2D(Collider2D other) {
		bool unsetClosestObject = false;
		if (other.gameObject.tag == tagToDetect || tagToDetect == "") {
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
					if (closestObject == null) {
						closestObject = other.gameObject;
					} else if(Vector3.Distance(transform.position, otherPosition) < Vector3.Distance(transform.position, closestObject.transform.position)) {
						closestObject = other.gameObject;
					}
				} else if (other.gameObject.Equals(closestObject)) {
					unsetClosestObject = true;
				}
			} else if (other.gameObject.Equals(closestObject)) {
				unsetClosestObject = true;
			}
		}
		if (unsetClosestObject) {
			closestObject = null;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.Equals(closestObject)) {
			closestObject = null;
		}
	}

	public GameObject getClosestDetectedObject() {
		return closestObject;
	}
}
