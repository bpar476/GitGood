﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour {

    private GameObject player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // Use this for initialization
    void Start () {
        this.player = GameObject.FindGameObjectWithTag ("Player");
    }

    // LateUpdate is called at end of Update Cycle
    void LateUpdate () {
        this.player = GameObject.FindGameObjectWithTag ("Player");
        // Camera can't go outside the clamp
        float x = Mathf.Clamp (this.player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp (this.player.transform.position.y, yMin, yMax);
        this.gameObject.transform.position = new Vector3 (x, y, this.gameObject.transform.position.z);
    }
}
