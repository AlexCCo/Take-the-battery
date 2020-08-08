using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {

    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpAltitud = 10f;

    [SerializeField] private GameObject groundCheck;
    // must be set to the layer of the elements your character can step on
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Animator animator;
    private float isIdleRightDirection;

    private Rigidbody2D characterBody;
    // x horizontal, y jumping
    private Vector2 movement;
    private Vector2 fixedMovement;
    private Vector2 zeroVector;
    private float movementSmoothingTime = 0.05f;

    private void Start() {
        characterBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movement = new Vector2(0, 0);
        fixedMovement = new Vector2(0, 0);
        zeroVector = Vector2.zero;
        isIdleRightDirection = 1f;
    }

    // Update is called once per frame
    void Update(){
        if (movement.x != 0) {
            // If the last horizontal move was 1, it means we were moving to the right
            // so our idle animation should be looking to the right.
            // Otherwise it means we are looking to the left
            isIdleRightDirection = movement.x;
        }
        
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", Math.Abs(movement.x) * speed);
        animator.SetFloat("isIdleRightDirection", isIdleRightDirection);
        
    }

    private void FixedUpdate() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Jump");

        Collider2D[] maxCollider = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundCheckRadius, groundLayer);

        if (maxCollider.Length > 0 && movement.y > 0) {
            fixedMovement.y = movement.y * jumpAltitud;
            characterBody.AddForce(fixedMovement, ForceMode2D.Impulse);
        }

        Vector2 targetVelocity = new Vector2(movement.x * speed * 10f, characterBody.velocity.y);
        Vector2 smoothVelocity = Vector2.SmoothDamp(characterBody.velocity, targetVelocity, ref zeroVector, movementSmoothingTime);

        characterBody.velocity = smoothVelocity;
    }

}
