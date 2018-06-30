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
		foreach (IVisionObserver observer in observers) {
			foreach (GameObject visibleObject in visibleObjects) {
				observer.ProcessVisibleObject(visibleObject);
			}
		}
	}

	// Is within radius
	private void OnTriggerStay2D(Collider2D other) {
		bool objectLeftVisibility = false;

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
				if (!visibleObjects.Contains(other.gameObject)) {
					visibleObjects.Add(other.gameObject);
					foreach (IVisionObserver observer in observers) {
						observer.ObjectEnteredVisibility(other.gameObject);
					}
				}
			} else if (visibleObjects.Contains(other.gameObject)) {
				objectLeftVisibility = true;
			}
		} else if (visibleObjects.Contains(other.gameObject)) {
			objectLeftVisibility = true;
		}
		if (objectLeftVisibility) {
			visibleObjects.Remove(other.gameObject);
			foreach (IVisionObserver observer in observers) {
				observer.ObjectLeftVisibility(other.gameObject);
			}
		}
	}

	/// <summary>
	/// Adds an IVisionObserver to the observers of this ConeDetector. Observers will be notified
	/// when objects enter the field of vision, leave the field of vision and every frame that they stay in the field of vision.
	/// </summary>
	/// <param name="observer">The observer to add.</param>
	public void AddObserver(IVisionObserver observer) {
		if (observer != null) {
			observers.Add(observer);
		}
	}

	/// <summary>
	/// Removes the given observer from this ConeDetector.
	/// </summary>
	/// <param name="observer"></param>
	public void RemoveObserver(IVisionObserver observer) {
		observers.Remove(observer);
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (visibleObjects.Contains(other.gameObject)) {
			visibleObjects.Remove(other.gameObject);
			foreach (IVisionObserver observer in observers) {
				observer.ObjectLeftVisibility(other.gameObject);
			}
		}
	}
}
