using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded = false;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 9.5f;
    private float movement = 0f;

    private bool facingRight = true;
    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void OnMove(InputValue value)
    {
        movement = value.Get<float>();
        Debug.Log(movement);
        if (movement < 0 && facingRight == true)
        {
            Flip();
        }

        if (movement > 0 && facingRight == false)
        {
            Flip();
        }
        
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    void Update()
    {
        Move(movement);
        animator.SetFloat("Speed", Mathf.Abs(speed * movement));

    }
    
    private void Move(float x)
    {
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Vector2 jumpAdder = new Vector2(rb.velocity.x, jumpHeight);
        rb.AddForce(jumpAdder, ForceMode2D.Impulse);
        animator.SetBool("IsJumping", true);
    }

    
    void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    // Ground Check 
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }


    
}