using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSlidingDoor : MonoBehaviour {

	public Animator anim;
	public Sprite closedSprite;
	
	private bool open = false;

	public void Open() {
		open = true;
		anim.SetBool("open", open);
	}

	private void Update() {
		if (anim.GetBool("open") && open) {
			GetComponent<SpriteRenderer>().sprite = closedSprite;
		}
	}
}
