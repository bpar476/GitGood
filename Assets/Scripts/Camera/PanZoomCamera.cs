using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoomCamera : MonoBehaviour {

	public float zoomSensitivity = 3.5f;

	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		float candidateNewSize = mainCamera.orthographicSize + zoomSensitivity * -1 * Input.GetAxis("Mouse ScrollWheel");
		mainCamera.orthographicSize = Mathf.Clamp(candidateNewSize, 0, candidateNewSize);
	}
}
