using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

    public int playerSpeed = 10;
    public int playerJumpPower = 1250;
    public int maxVelocity = 10;
    private bool facingRight = true;
    private float moveX;
    private bool isAirborn = false;

    // Update is called once per frame
    void Update () {
        PlayerMove ();
    }

    void PlayerMove () {
        //CONTROLS
        if (!this.isAirborn) {
            this.moveX = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown ("Jump")) {
                Jump ();
                this.isAirborn = true;
            }
            //ANIMATIONS
            //PLAYER DIRECTION
            if ((this.moveX < 0.0f && this.facingRight) || (this.moveX > 0.0f && !this.facingRight)) {
                FlipPlayer ();
            }
            //PHYSICS
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * this.playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        } else if (this.gameObject.GetComponent<Rigidbody2D>().velocity.y == 0){
            
        }
    }

    void Jump () {
        //JUMPING
        GetComponent<Rigidbody2D> ().AddForce (Vector2.up * this.playerJumpPower, ForceMode2D.Impulse);
    }

    void FlipPlayer () {
        this.facingRight = !this.facingRight;
        Vector2 localScale = this.gameObject.transform.localScale;
        localScale.x *= -1;
        this.transform.localScale = localScale;
    }
}
