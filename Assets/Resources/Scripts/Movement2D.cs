using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpAltitud = 10f;
    private Animator animator;
    private Rigidbody2D characterBody;
    // x horizontal, y jumping
    private Vector2 movement;
    private float isIdleRightDirection;

    private void Start() {
        characterBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Vector2 movement = new Vector2(0, 0);
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

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Jump");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", Math.Abs(movement.x) * speed);
        animator.SetFloat("isIdleRightDirection", isIdleRightDirection);
        
    }

    private void FixedUpdate() {
        Debug.Log("Horizontal: " + movement.x + "  jump: " + movement.y);

        movement.x *= speed;
        movement.y *= jumpAltitud;
        characterBody.MovePosition(characterBody.position + movement * Time.fixedDeltaTime);
    }
}
