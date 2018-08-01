using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTreeControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Destroy(gameObject);
			EngineController.Instance().ToggleControls(true);
		}
	}
}
