using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveForce;
    public float jumpForce;
    public float maxVelocity;
    public Transform groundCheck;
    public LayerMask groundCheckLayer;
    public Vector2 forward {
        get { 
            if (facingRight) {
                return new Vector2(1,0);
            } else {
                return new Vector2(-1,0);
            }
        }
    }

    private bool facingRight = true;
    private bool jump = false;
    private Rigidbody2D myRigidBody;

    void Awake() {
        myRigidBody = gameObject.GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        float groundCheckDistance = GetComponent<Collider2D>().bounds.extents.y + 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        

        if (Input.GetButtonDown("Jump") && raycastHit.collider != null) {
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
        if (horizontalInput * myRigidBody.velocity.x < maxVelocity) {
            myRigidBody.AddForce(Vector2.right * horizontalInput * moveForce);
        }
        if (Mathf.Abs(myRigidBody.velocity.x) > maxVelocity) {
            myRigidBody.velocity = new Vector2(Mathf.Sign(myRigidBody.velocity.x) * maxVelocity, myRigidBody.velocity.y);
        }
    }

    private void UpdateJump () {
        if (jump) {
            myRigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jump = false;
        }
    }

    public void MoveTo(Vector3 target, Action then) {
       StartCoroutine(doMoveTo(target, then));
    }

    private IEnumerator doMoveTo(Vector3 target, Action then) {
        while (Mathf.Abs(transform.position.x - target.x) > 0.1) {
            Vector3 oldPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), 0.09f);
            yield return null;
            if (Mathf.Abs(oldPosition.x - transform.position.x) < 0.01) {
                break;
            }
        }
        then();
    }
}
