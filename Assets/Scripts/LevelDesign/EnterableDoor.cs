using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterableDoor : MonoBehaviour {

	private static readonly float enterDistance = 0.5f;

	public Animator doorAnimator;

	public void TryEnter(GameObject enterer) {
		if (Vector3.Distance(transform.position, enterer.transform.position) < enterDistance) {
			EnterDoor enterDoor = enterer.GetComponent<EnterDoor>();
			if (enterDoor != null) {
				doorAnimator.SetBool("open", true);
				enterDoor.Enter(this);
			}
		}
	}
}
