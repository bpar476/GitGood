using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	public Collider2D attachedBody;
	public PersistentInteractable currentInteraction;

	private void Update() {
		if(Input.GetKeyDown(KeyCode.F)) {
			if (currentInteraction == null) {
				// We need to do this so that we can detect doors that we are in front of
				// By default it is false because it messes up other raycasts
				Physics2D.queriesStartInColliders = true;
				PlayerMovement movement = GetComponent<PlayerMovement>();

				RaycastHit2D[] results = new RaycastHit2D[10];
				attachedBody.Cast(movement.forward, results);
				Physics2D.queriesStartInColliders = false;

				for (int i = 0; i < results.Length; i++) {
					if (results[i] != null) {
						Collider2D collidedWith = results[i].collider;
						if (collidedWith != null) {
							GameObject hit = collidedWith.gameObject;
							Interactable interactable = hit.GetComponent<Interactable>();
							if (interactable != null) {
								if(interactable.TryInteract(gameObject)) {
									PersistentInteractable persistence = hit.GetComponent<PersistentInteractable>();
									if (persistence != null) {
										currentInteraction = persistence;
									}
									break;
								}
							}
						}
					}
				}
			}
		} else {
			currentInteraction.StopInteracting();
		}
	}
}
