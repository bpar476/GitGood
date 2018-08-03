using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterableDoor : Interactable {

	public Triggerable trigger;

	private static readonly float enterDistance = 1.0f;

	public Animator doorAnimator;

	public override bool TryInteract(GameObject enterer) {
		if (Vector3.Distance(transform.position, enterer.transform.position) < enterDistance) {
			EnterDoor enterDoor = enterer.GetComponent<EnterDoor>();
			if (enterDoor != null) {
				StartCoroutine(LetEnter(enterDoor));
				return true;
			}
		}
		return false;
	}

	private IEnumerator LetEnter(EnterDoor enterer) {
		doorAnimator.SetBool("open", true);

		yield return new WaitForSeconds(0.5f);

		enterer.Enter(this);

		yield return new WaitForSeconds(1);

		if (trigger != null) {
			trigger.NotifyObservers();
		}
	}
}
