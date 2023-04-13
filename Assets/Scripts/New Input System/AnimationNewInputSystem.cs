using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationNewInputSystem : MonoBehaviour
{
    private Animator animator;
    public NewPlayerMovement newPlayerMovement;
    public Rigidbody2D rb;
    private string currentAnimation;

    private float horizontal;
    private float vertical;

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
    }


    // Update is called once per frame
    void Update()
    {
        // Player idle animation
        if (rb.velocity.x == 0 && rb.velocity.y == 0 && newPlayerMovement.IsGrounded())
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        // Run animation is added when player gain velocity in x axis and not depended on input system.
        if (rb.velocity.y == 0 && newPlayerMovement.IsGrounded() && rb.velocity.x != 0) 
        {
            ChangeAnimationState(PLAYER_RUN);
        }
    }
    

    // Adds jump animation.
    public void jumpAnimation(InputAction.CallbackContext context)
    {
        if (context.performed && newPlayerMovement.IsGrounded())
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        
        if (context.performed && !newPlayerMovement.IsGrounded())
        {
           ChangeAnimationState(PLAYER_DOUBLE_JUMP);
        }
    }


    public void dashAnimation(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            ChangeAnimationState(PLAYER_DOUBLE_JUMP);
        }
    }


    void ChangeAnimationState(string newState)
    {
        // stops interuption between animations
        if (currentAnimation == newState) return;

        animator.Play(newState);

        // reassign current state
        currentAnimation = newState;
    }
}
