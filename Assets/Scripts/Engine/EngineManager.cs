using System;
using UnityEngine;

public class EngineController {
    // Singleton
	private static EngineController singletonInstance;
    private bool controlsEnabled = true;

    public static EngineController Instance() {
		if (singletonInstance == null) {
			EngineController.Reset();
		}
		return singletonInstance;
	}
	public static void Reset() {
		singletonInstance = new EngineController();
	}

    public void ToggleControls(bool enabled) {
        controlsEnabled = enabled;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().enabled = controlsEnabled;
        GameObject.FindGameObjectWithTag("Player").GetComponent<VersionControls>().enabled = controlsEnabled;
        GameObject.Find("/EngineController/UIController").GetComponent<UIController>().enabled = controlsEnabled;
    }
    
    public bool ControlsEnabled() {
        return controlsEnabled;
    }
}
