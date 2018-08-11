using System.Collections.Generic;
using System;
using UnityEngine;

public class DirectionBinaryVersionable : MonoBehaviour, IBinaryVersionable {

    public bool initialState;
    public float scaleValue;

    private bool stagedState;
    private History<bool> history;

    private void Awake() {
        this.history = new History<bool>();
        if (this.scaleValue < 0) {
            this.scaleValue *= -1;
        }
    }

    public void Stage(GameObject version) {
        this.stagedState = version.transform.localScale.x >= 0;
    }

    public void Commit(IVersion version) {
        this.history.Add(version, stagedState);
    }

    public void ResetToVersion(IVersion version, GameObject target) {
        this.Reset(history.GetStateAt(version), target);
    }

    public void ResetToStaged(GameObject target) {
        this.Reset(stagedState, target);
    }

    public void ResetToInitialState(GameObject target) {
        this.Reset(this.initialState, target);
    }

    public bool GetState() {
        return stagedState;
    }

    public void SetScaleValue(float value) {
        if (value < 0) {
            value *= -1;
        }
        this.scaleValue = value;
    }

    public void SetInitialState(bool value) {
        this.initialState = value;
    }

    private void Reset(bool state, GameObject target) {
        float value = state ? scaleValue : -1 * scaleValue;
        target.transform.localScale = new Vector3(value, target.transform.localScale.y, target.transform.localScale.z);
    }

    public string DescribeState(IVersion version) {
        string direction = history.GetStateAt(version) ? "right" : "left";
        return "Direction: " + direction;
    }

    public string DescribeStagedState() {
        string direction = stagedState ? "right" : "left";
        return "Direction: " + direction;
    }

}