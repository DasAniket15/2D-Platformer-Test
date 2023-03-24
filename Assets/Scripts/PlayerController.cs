using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed;

    [SerializeField] private float jumpPower;
    [SerializeField] private float doubleJumpPower;
    [SerializeField] private float maxFallSpeed;

    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private float afterDashCooldown;
    [SerializeField] private float dashPushPower ;

    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [SerializeField] private float coyoteTimeChecker;
    public bool coyoteTimeCheckerBool;
    private float coyoteTimeCheckerCounter;


    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;

    public bool isFacingRight = true;
    private bool doubleJump;

    private bool canDash = true;
    public bool isDashing;
    private bool hasDashed;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;


    void Update()
    {
        // Coyote time
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }

        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            Debug.Log(coyoteTimeCounter);

        }

        // Coyote time checker
        if (IsGrounded())
        {
            coyoteTimeCheckerCounter = coyoteTimeChecker;


        }

        else
        {
            coyoteTimeCheckerCounter -= Time.deltaTime;
            Debug.Log(coyoteTimeCheckerCounter);
        }

        if (coyoteTimeCheckerCounter > 0)
        {
            coyoteTimeCheckerBool = true;
        }
        else
        {
            coyoteTimeCheckerBool = false;
        }


        // Jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }

        // Jump and double jump
        horizontal = Input.GetAxis("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpPower : jumpPower);

                jumpBufferCounter = 0f;
                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // Applies momentum to character after dash
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (hasDashed == true && isFacingRight && !IsGrounded())
        {
            rb.AddForce(new Vector2(dashPushPower, rb.velocity.y));
        }

        if (hasDashed == true && !isFacingRight && !IsGrounded())
        {
            rb.AddForce(new Vector2(-dashPushPower, rb.velocity.y));
        }

        // Clamps fall velocity
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;

            transform.localScale = localScale;
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        hasDashed = true;

        yield return new WaitForSeconds(afterDashCooldown);
        hasDashed = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}

//code to be used later
/*
    [SerializeField] private float coyoteTimeChecker;
    public bool coyoteTimeCheckerBool;
    private float coyoteTimeCheckerCounter;
 
     if (IsGrounded())
        {
            coyoteTimeCheckerCounter = coyoteTimeChecker;
           
           
        }

        else
        {
            coyoteTimeCheckerCounter -= Time.deltaTime;
            Debug.Log(coyoteTimeCheckerCounter);
        }

        if (coyoteTimeCheckerCounter > 0)
        {
            coyoteTimeCheckerBool = true;
        }
        else 
        {
            coyoteTimeCheckerBool = false;
        }
 */