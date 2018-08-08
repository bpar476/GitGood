using System;
using UnityEngine;

public class EngineController : Singleton<EngineController> {
    private bool controlsEnabled = true;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private EngineController() {

    }
    public void ToggleControls(bool enabled) {
        controlsEnabled = enabled;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = controlsEnabled;
        GameObject.FindGameObjectWithTag("Player").GetComponent<VersionControls>().enabled = controlsEnabled;
        GameObject.Find("/EngineController/UIController").GetComponent<UIController>().enabled = controlsEnabled;
    }

    public void ToggleMovement(bool enabled) {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = enabled;
    }
    
    public bool ControlsEnabled() {
        return controlsEnabled;
    }
}
