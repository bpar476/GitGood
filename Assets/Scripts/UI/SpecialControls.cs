using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			foreach (Transform child in transform) {
				Destroy(child.gameObject);
				EngineController.Instance().ToggleControls(true);
			}
		}
	}
}
