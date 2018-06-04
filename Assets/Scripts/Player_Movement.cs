using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

    public int playerSpeed = 10;
    public bool facingRight = true;
    public int playerJumpPower = 1250;
    public float moveX;

    // Update is called once per frame
    void Update () {
        PlayerMove ();
    }

    void PlayerMove () {
        //CONTROLS
        this.moveX = Input.GetAxis("Horizontal");
        //ANIMATIONS
        //PLAYER DIRECTION
        if ((this.moveX < 0.0f && this.facingRight) || (this.moveX > 0.0f && !this.facingRight)) {
            FlipPlayer ();
        }
        //PHYSICS
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * this.playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    void Jump () {
        //JUMPING
    }

    void FlipPlayer () {
        this.facingRight = !this.facingRight;
        Vector2 localScale = this.gameObject.transform.localScale;
        localScale.x *= -1;
        this.transform.localScale = localScale;
    }
}
