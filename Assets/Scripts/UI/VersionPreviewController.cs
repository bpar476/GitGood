using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VersionPreviewController : MonoBehaviour {

	public GameObject previewObject;
	public Canvas canvas;
	public Button actionButton;
	public Text title;
	public Text versionablePropertyOne;
	public Text versionablePropertyTwo;
	public VersionController versionedObject;

	private Vector2 uiOffset;
	private RectTransform canvasRect;

	private void Start() {
		canvas = GameObject.Find("Overlay").GetComponent<Canvas>();
		if (canvas == null) {
			Debug.Log("CANVAS NOT FOUND");
		}
		this.canvasRect = canvas.GetComponent<RectTransform>();
		uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f);
		actionButton.onClick.AddListener(OnActionButtonClicked);
		title.text = versionedObject.objectName;

		this.transform.SetParent(canvas.transform);

		string[] description = versionedObject.DescribeStagedState().Split('\n');
		Debug.Log(description);
		if (description.Length >= 1) {
			versionablePropertyOne.text = description[0];
		}
		if (description.Length == 2) {
			versionablePropertyTwo.gameObject.SetActive(true);
			versionablePropertyTwo.text = description[1];
		}

	}

	public void SetPreviewObject(VersionController versionedObject, GameObject preview) {
		this.previewObject = preview;
		this.versionedObject = versionedObject;
	}

	private void Update() {
		if (previewObject != null) {
			// Taken from stack overflow: https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
			Vector3 targetPosition = this.previewObject.transform.position + new Vector3(-1.0f, 1.5f, 0);
			Vector2 uiPosition = Camera.main.WorldToViewportPoint(targetPosition);
			Vector2 proportionalPosition = new Vector2(uiPosition.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x*0.5f
												  	  ,uiPosition.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y*0.5f);
			GetComponent<RectTransform>().anchoredPosition = proportionalPosition;
		}
	}

	protected abstract void OnActionButtonClicked();
}
