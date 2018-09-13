using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapTrigger : TriggerObserver {

	public Sprite defaultSprite;
	public Sprite otherSprite;
	
	protected override void HandleTrigger(bool state) {
		if (state) {
			GetComponent<SpriteRenderer>().sprite = otherSprite;
		} else {
			GetComponent<SpriteRenderer>().sprite = defaultSprite;
		}
	}
}
