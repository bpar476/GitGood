using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButtonDialogControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Destroy(gameObject);
			EngineController.Instance().ToggleControls(true);
		}
		else if (Input.GetKeyDown(KeyCode.Return)) {
			if (gameObject.GetComponent<TextButtonDialogController>().submitButton.enabled) {
				gameObject.GetComponent<TextButtonDialogController>().submitButton.onClick.Invoke();
			}
		}
	}
}
