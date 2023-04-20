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
        }
    }


    private void setFalse(bool hasLanded) 
    {
        this.hasLanded = false;
    }
    

    // Adds jump animation.
    public void jumpAnimation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           ChangeAnimationState(PLAYER_JUMP);
            
            if (!newPlayerMovement.IsGrounded()) 
            {
                ChangeAnimationState(PLAYER_DOUBLE_JUMP);
            }
        }

        if (context.canceled)
        {
            hasLanded = true;
            
            Debug.Log("Cancelled");
        }
    }


    public void runAnimation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeAnimationState(PLAYER_RUN);
        }

        if (context.canceled) 
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }


    public void dashAnimation(InputAction.CallbackContext context) 
    {
        
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
