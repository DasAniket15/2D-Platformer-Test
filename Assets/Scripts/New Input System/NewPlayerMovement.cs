using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class NewPlayerMovement : MonoBehaviour, PlayerControls.IMovementActions
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    private PlayerControls playerControls;

    private float horizontal;
    [SerializeField] private float speed;

    private bool doubleJump;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float doubleJumpingPower;
    [SerializeField] private float maxFallSpeed;

    private bool canDash = true;
    private bool isDashing;
    private bool hasDashed;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private float afterDashTime;
    [SerializeField] private float dashPushingPower;

    private float coyoteTimeCounter;
    [SerializeField] private float coyoteTime;

    private bool coyoteTimeCheckBool;
    private float coyoteTimeCheckCounter;
    [SerializeField] private float coyoteTimeCheck;

    private float jumpBufferCounter;
    [SerializeField] private float jumpBufferTime;

    private bool isFacingRight = true;


    private void Awake()
    {
        playerControls = new PlayerControls();

        playerControls.Movement.SetCallbacks(this);
    }


    // Update is called once per frame
    void Update()
    {
        // Coyote Time
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }

        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Coyote Time Check
        if (IsGrounded())
        {
            coyoteTimeCheckCounter = coyoteTimeCheck;
        }

        else
        {
            coyoteTimeCheckCounter -= Time.deltaTime;
        }

        if (coyoteTimeCheckCounter > 0)
        {
            coyoteTimeCheckBool = true;
        }

        else
        {
            coyoteTimeCheckBool = false;
        }

        // Smoothens Player Movement
        horizontal = UnityEngine.Input.GetAxis("Horizontal");
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // Horizontal Movement
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }

        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        // Applies momentum to character after dash
        if (hasDashed == true && isFacingRight && !IsGrounded())
        {
            rb.AddForce(new Vector2(dashPushingPower, rb.velocity.y));
        }

        if (hasDashed == true && !isFacingRight && !IsGrounded())
        {
            rb.AddForce(new Vector2(-dashPushingPower, rb.velocity.y));
        }

        // Clamp Fall Velocity
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }


    // Input System Method for Horizontal Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }


    // Input System Method for Jump Mechanics
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDashing)
        {
            return;
        }

        // Jump Buffer
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump and Double Jump Logic
        if (context.performed && IsGrounded())
        {
            doubleJump = false;
        }

        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);

                jumpBufferCounter = 0f;
                doubleJump = !doubleJump;
            }
        }

        if (context.canceled && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            coyoteTimeCounter = 0f;
        }
    }

    
    // Input System Method for Dash Mechanic
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    

    public IEnumerator Dash()
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

        yield return new WaitForSeconds(afterDashTime);
        hasDashed = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    // Checks if player is grounded or not
    public bool IsGrounded()
    {
         return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
       // return Physics2D.O
    }


    // Flips character sprite to face correct direction
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;

        transform.localScale = localScale;
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }
}
