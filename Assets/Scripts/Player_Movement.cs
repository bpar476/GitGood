using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour {

    public float moveForce;
    public float jumpForce;
    public float maxVelocity;
    public float horizontalDecelaration;

    public Transform groundCheck;
    public LayerMask groundCheckLayer;

    private bool facingRight = true;
    private bool isAirborn = false;
    private bool jump = false;
    private Rigidbody2D myRigidBody;

    void Awake() {
        myRigidBody = gameObject.GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        isAirborn = !Physics2D.Linecast(transform.position, groundCheck.position, groundCheckLayer);

        if (Input.GetButtonDown("Jump") && !isAirborn) {
            jump = true;
        }
    }

    // Called in sync with physics engine
    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");

        UpdateDirection(h);
        UpdateHorizontalVelocity(h);
        UpdateJump();

    }
    private void UpdateDirection(float horizontalInput) {
        if ((horizontalInput < 0.0f && this.facingRight) || (horizontalInput > 0.0f && !this.facingRight)) {
            this.facingRight = !this.facingRight;
            Vector2 localScale = this.gameObject.transform.localScale;
            localScale.x *= -1;
            this.transform.localScale = localScale;
        }
    }

    private void UpdateHorizontalVelocity(float horizontalInput) {
        float currentXSpeed = Mathf.Abs(myRigidBody.velocity.x);
        if (horizontalInput == 0 && currentXSpeed > 0.1) {
            float direction = Mathf.Sign(myRigidBody.velocity.x);
            myRigidBody.AddForce(Vector2.right * direction * -1 * horizontalDecelaration);
        } else {
            if (horizontalInput * myRigidBody.velocity.x < maxVelocity) {
                myRigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
            }
            if (Mathf.Abs(myRigidBody.velocity.x) > maxVelocity) {
                myRigidBody.velocity = new Vector2(Mathf.Sign(myRigidBody.velocity.x) * maxVelocity, myRigidBody.velocity.y);
            }
        }
    }

    private void UpdateJump () {
        if (jump) {
            myRigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jump = false;
        }
    }
}
