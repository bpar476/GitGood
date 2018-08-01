using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour {

	public void Enter(EnterableDoor door) {
		PlayerMovement movement = GetComponent<PlayerMovement>();
		movement.MoveTo(door.transform.position, DoEnterDoor);
	}

	private void DoEnterDoor() {
		StartCoroutine(FadeIntoDoor());
	}

	private IEnumerator FadeIntoDoor() {
		Renderer renderer = GetComponent<Renderer>();
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			yield return null;
		}
	}
}
