using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{

    Animator animator;
    private Rigidbody2D rb;
    private Transform groundCheck;
    public PlayerController playerController;

    private string currentAnimation;
    private float horizontal;
    private float vertical;
    private bool canRun;
    private bool showDash = true;



    // Animation States
    const string PLAYER_IDLE = "playerIdle";
    const string PLAYER_RUN = "playerRun";
    const string PLAYER_JUMP = "playerJump";
    const string PLAYER_DOUBLE_JUMP = "playerDoubleJump";
    const string PLAYER_DASH = "playerDash";

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        groundCheck = transform.parent.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 && playerController.IsGrounded())
        {
            canRun = true;
            //ChangeAnimationState(PLAYER_RUN);
           // Debug.Log("Yes");
        }
        if (horizontal == 0 && playerController.isDashing == false)
        {
            ChangeAnimationState(PLAYER_IDLE);
            
        }
        if (Input.GetButtonDown("Jump") && playerController.IsGrounded() )
        {
            animator.Play(PLAYER_JUMP);

           // Debug.Log("Yes");

        }
        if (Input.GetButtonDown("Jump") && !playerController.IsGrounded() )
        {
            if (playerController.coyoteTimeCheckerBool == false)
            {
                animator.Play(PLAYER_DOUBLE_JUMP);
            }
            else
            {
                animator.Play(PLAYER_JUMP);
            }
           // Debug.Log("Yes");
        }

        if (vertical == 0 && playerController.IsGrounded())
        {
            if (canRun == true)
            {
                animator.Play(PLAYER_RUN);
                canRun = false;
            }
            else
            {
                animator.Play(PLAYER_IDLE);
            }
            //Debug.Log("Yes");

        }
        if(playerController.isDashing == true && !playerController.IsGrounded())
        {
            ChangeAnimationState(PLAYER_DOUBLE_JUMP);
            //showDash = false;
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