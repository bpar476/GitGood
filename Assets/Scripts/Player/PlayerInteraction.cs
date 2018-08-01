﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	public Rigidbody2D attachedBody;

	private void Update() {
		if(Input.GetKeyDown(KeyCode.F)) {
			// We need to do this so that we can detect doors that we are in front of
			// By default it is false because it messes up other raycasts
			Physics2D.queriesStartInColliders = true;
			PlayerMovement movement = GetComponent<PlayerMovement>();

			RaycastHit2D[] results = new RaycastHit2D[4];
			attachedBody.Cast(movement.forward, results);
			Physics2D.queriesStartInColliders = false;

			for (int i = 0; i < results.Length; i++) {
				Collider2D collidedWith = results[i].collider;
				if (collidedWith != null) {
					GameObject hit = collidedWith.gameObject;
					EnterableDoor door = hit.GetComponent<EnterableDoor>();
					if (door != null) {
						if(door.TryEnter(gameObject)) {
							break;
						}
					}
				}
			}
		}
	}
}
