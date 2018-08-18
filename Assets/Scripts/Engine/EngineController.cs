using System;
using UnityEngine;

public class EngineController : Singleton<EngineController> {
    private bool controlsEnabled = true;
    private GameObject player;

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private EngineController() {

    }

    public void ToggleControls(bool enabled) {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        controlsEnabled = enabled;
        player.GetComponent<PlayerMovement>().enabled = controlsEnabled;
        player.GetComponent<VersionControls>().enabled = controlsEnabled;
        player.GetComponent<PlayerInteraction>().enabled = controlsEnabled;
        GameObject.Find("/EngineController/UIController").GetComponent<UIController>().enabled = controlsEnabled;
    }

    public void ToggleMovement(bool enabled) {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<PlayerMovement>().enabled = enabled;
    }
    
    public bool ControlsEnabled() {
        return controlsEnabled;
    }
}
