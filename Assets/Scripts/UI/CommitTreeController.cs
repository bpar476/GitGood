using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommitTreeController : MonoBehaviour {
	public GameObject commitTreePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowTree() {
		EventSystem.current.SetSelectedGameObject(GameObject.Find("/EngineController/UIController"));
		EngineController.Instance().ToggleControls(false);
		GameObject popup = Instantiate(commitTreePrefab, GameObject.Find("UIController").transform) as GameObject;
	}
}
