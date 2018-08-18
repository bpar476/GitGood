using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		text.enabled = false;
	}

	public void Flash() {
		text.enabled = true;
		StartCoroutine(FadeAwayText());
	}

	private IEnumerator FadeAwayText() {
		yield return new WaitForSeconds(2);

		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = text.color;
			c.a = f;
			text.color = c;
			yield return null;
		}

		text.enabled = false;
		Color color = text.color;
		color.a = 1f;
		text.color = color;

		yield return null;
	}
}
