using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoomCamera : MonoBehaviour {

	public float zoomSensitivity = 3.5f;
	public float mouseSensitivity = 0.015f;

	private Camera mainCamera;
	private Vector3 lastPosition;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		float candidateNewSize = mainCamera.orthographicSize + zoomSensitivity * -1 * Input.GetAxis("Mouse ScrollWheel");
		mainCamera.orthographicSize = Mathf.Clamp(candidateNewSize, 0, candidateNewSize);

		// Taken from Unity Forum: https://answers.unity.com/questions/614288/pan-camera-with-mouse.html
		if (Input.GetMouseButtonDown(0)) {
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0)) {
			Vector3 delta = Input.mousePosition - lastPosition;
			mainCamera.transform.Translate(-delta.x * mouseSensitivity, -delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}
	}
}
