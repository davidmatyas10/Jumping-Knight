using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMovement;
    
    [Header("Jumping")]
    public float jumpPower = 10f;
    public float holdJumpMultiplier = 1.3f;
    
    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public Transform platformCheckPos;
    public Vector2 platformCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    bool isGrounded = true;
    bool isPlatform = true;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f;
    public float fallSpeedMultiplier = 2f;

    void Update()
    {
        GroundCheck();
        PlatformCheck();
        ProcessGravity();
        
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        Flip();
        
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        
        // Update jump animation
        animator.SetBool("jump", !isGrounded && !isPlatform); // If the player is not on ground, sets jump = true
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && (isGrounded || isPlatform)) // Allow jumping only if the player is on the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
        else if (context.performed && (isGrounded || isPlatform))
        { 
            rb.velocity = new Vector2(rb.velocity.x, jumpPower * holdJumpMultiplier);
            animator.SetTrigger("jump");
        }   
        else if(context.canceled && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            animator.SetTrigger("jump");
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
    }

    private void PlatformCheck()
    {
        isPlatform = Physics2D.OverlapBox(platformCheckPos.position, platformCheckSize, 0, platformLayer);
    }

    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(platformCheckPos.position, platformCheckSize);
    }
}