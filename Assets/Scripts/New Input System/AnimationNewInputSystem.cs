using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;

public class AnimationNewInputSystem : MonoBehaviour
{
    private Animator animator;
    public NewPlayerMovement newPlayerMovement;
    public Rigidbody2D rb;
    private string currentAnimation;

    private float horizontal;
    private float afterJump = 0.6f;
    private int counter = 0;
    private bool hasLanded;
    private bool isRunning;
    

    // Animation States
    const string PLAYER_IDLE = "playerIdle";
    const string PLAYER_RUN = "playerRun";
    const string PLAYER_JUMP = "playerJump";
    const string PLAYER_DOUBLE_JUMP = "playerDoubleJump";
    const string PLAYER_DASH = "playerDash";


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(PLAYER_IDLE);
    }


    // Update is called once per frame
    void Update()
    {
        if (newPlayerMovement.IsGrounded())
        {
           if (hasLanded == true)
            {
                ChangeAnimationState(PLAYER_IDLE);
                setFalse(hasLanded);
                
                Debug.Log("Yes has landed");
            }

            if (rb.velocity.x != 0) 
            {
                ChangeAnimationState(PLAYER_RUN);
            }

            if (rb.velocity.x == 0)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        if (!newPlayerMovement.IsGrounded()){
            if (rb.velocity.y != 0 && rb.velocity.y >= -10f)
            {
                ChangeAnimationState(PLAYER_JUMP);
            }

            if (rb.velocity.y < -10f)
            {
                ChangeAnimationState(PLAYER_DOUBLE_JUMP);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
                // Handle collision with wall
                ChangeAnimationState(PLAYER_IDLE);

                Debug.Log("collided");
        }
    }

    private void setFalse(bool hasLanded) 
    {
        this.hasLanded = false;
    }

    // Adds jump animation.
    public void jumpAnimation(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            if (!newPlayerMovement.IsGrounded()) 
            {
                ChangeAnimationState(PLAYER_DOUBLE_JUMP);
            }
        }

        if (context.canceled && newPlayerMovement.IsGrounded())
        {
            hasLanded = true;

            ChangeAnimationState(PLAYER_IDLE);

            Debug.Log("Cancelled");
        }
    }

    public void runAnimation(InputAction.CallbackContext context)
    {

    }


    public void dashAnimation(InputAction.CallbackContext context) 
    {
        
    }

    void ChangeAnimationState(string newState)
    {
        // stops interruption between animations
        if (currentAnimation == newState) return;

        animator.Play(newState);

        // reassign current state
        currentAnimation = newState;
    }
}
