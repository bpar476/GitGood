using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickerImage : MonoBehaviour {

	public float maxPeriod = 0.8f;
	public float minPeriod = 0.2f;
	public float maxDelay = 0.8f;
	public float minDelay = 0.1f;

	private bool fading = false;
	private bool decreasing = false;
	private System.Random rng;
	private Image image;
	private float period;
	private float delay;
	private float progress;
	private bool waiting = false;

	private void Start() {
		this.rng = new System.Random();
		this.image = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		if (!this.fading && !this.waiting) {
			Debug.Log("Starting cycle");
			if (this.decreasing) {
				Debug.Log("Fading");
			} else {
				Debug.Log("Appearing");
			}
			this.period = (float)(this.rng.NextDouble() * (maxPeriod - minPeriod)) + minPeriod;
			this.progress = this.period;
			this.fading = true;
		} else if (this.fading && this.progress > 0) {
			Debug.Log("Resetting alpha");
			this.progress -= Time.deltaTime;
			float proportion = this.progress / this.period;
			Color color = this.image.color;
			float alpha = proportion;
			if (!this.decreasing) {
				alpha = 1 - proportion;
			}
			this.image.color = new Color(color.r, color.g, color.b, alpha);
		} else if (this.waiting && this.progress > 0) {
			Debug.Log("Waiting");
			this.progress -= Time.deltaTime;
		} else if(this.waiting) {
			this.waiting = false;
		} else {
			if (!this.decreasing) {
				Debug.Log("Setting Delay");
				this.waiting = true;
				this.delay = (float)(this.rng.NextDouble() * (maxDelay - minDelay)) + minDelay;
				this.progress = delay;
			}
			Debug.Log("Flipping direction and unsetting fading");
			this.decreasing = !decreasing;
			this.fading = false;
		}
	}
}
