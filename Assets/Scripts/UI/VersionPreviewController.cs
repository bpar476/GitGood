using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionPreviewController : MonoBehaviour {

	public GameObject previewObject;
	public Canvas canvas;

	private Vector2 uiOffset;
	private RectTransform canvasRect;

	private void Start() {
		Canvas canvas = GetComponentInParent<Canvas>();
		this.canvasRect = canvas.GetComponent<RectTransform>();
		uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f);
	}


	public void SetPreviewObject(GameObject preview) {
		this.previewObject = preview;
	}

	private void Update() {
		if (previewObject != null) {
			Vector3 targetPosition = this.previewObject.transform.position + new Vector3(-1.0f, 1.5f, 0);
			Vector2 uiPosition = Camera.main.WorldToViewportPoint(targetPosition);
			Vector2 proportionalPosition = new Vector2(uiPosition.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x*0.5f
												  	  ,uiPosition.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y*0.5f);
			GetComponent<RectTransform>().anchoredPosition = proportionalPosition;
		}
	}
}
