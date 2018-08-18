using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeInterfaceCamera : MonoBehaviour {

	public float xPosition;
	public float yPosition;
	public float size;

	private float originalSize;

	private void OnEnable() {
		EngineController.Instance().ToggleControls(false);
		Camera.main.GetComponent<PlayerCamera>().enabled = false;
		Camera.main.transform.position = new Vector3(xPosition, yPosition, -1);
		originalSize = Camera.main.orthographicSize;
		Camera.main.orthographicSize = size;

		GetComponent<PanZoomCamera>().enabled = true;
	}

	private void OnDisable() {
		EngineController.Instance().ToggleControls(true);
		Camera.main.orthographicSize = originalSize;
		GetComponent<PanZoomCamera>().enabled = false;
		Camera.main.GetComponent<PlayerCamera>().enabled = true;
	}
}
