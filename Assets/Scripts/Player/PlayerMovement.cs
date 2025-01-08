//Script that handels the Player Movement (W,A,S,D, Jump)

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class PlayerMovement : MonoBehaviour
{
    Vector3 playerInput, moveVector;

    Rigidbody rb;
    CharacterController Controller;

    GroundCheck groundCheck;

    SpriteRenderer sR;

    Animator animator;

    bool facingRight = true;
    bool canMove, canBlock;

    [SerializeField] float playerSpeed, jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundCheck = gameObject.transform.GetChild(0).GetComponent<GroundCheck>();
        canMove = true;
        canBlock = true;
    }

    void Update()
    {
        if (canMove)
        {
            PlayerMove();
        }
        if (canBlock)
        {
            PlayerBlock();
        }
        PlayerAnimation();
        PlayerFlip();
    }

    void PlayerMove()
    {
        //Get the Inputs from the player
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.z = Input.GetAxisRaw("Vertical");

        //Cancels out movement if both of the same keys are pressed depending on which axis
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            playerInput.x = 0;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            playerInput.z = 0;
        }

        //Apply the playerInput to the Rigidbody
        moveVector = transform.TransformDirection(playerInput) * playerSpeed;
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);

        //If the player if going Diagonally,
        //set the moveVector to the original speed and not double it
        if (rb.velocity.magnitude > 1)
        {
            moveVector.Normalize();
        }

        //If the player presses the jump key and the player is grounded,
        //add a force upwards and set animation trigger of Jump
        if (Input.GetKeyDown(KeyCode.Space) && groundCheck.grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    void PlayerBlock()
    {
        if (groundCheck.grounded && Input.GetKeyDown(KeyCode.E) && animator.GetBool("IdleBlock") == true)
        {
            animator.SetTrigger("Block");
        }
    }

    void PlayerAnimation()
    {
        //Set AnimState 1 if the player is moving, else set to 0
        if(moveVector.x != 0 || moveVector.z != 0)
        {
            animator.SetFloat("AnimState", 1);
            animator.SetBool("IdleBlock", false);
        }
        else
        {
            animator.SetFloat("AnimState", 0);
            animator.SetBool("IdleBlock", true);
        }

        //Set grounded to true if the player is on the ground
        if (groundCheck.grounded)
        {
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
        }

        //Set AirSpeedY to the players Y velocity to check if the player is fallling
        animator.SetFloat("AirSpeedY", rb.velocity.y);
    }

    void PlayerFlip()
    {
        //If the player is pressing the opposite key and the sprite is in the wrong direction,
        //Flip the sprite and set facingRight to the opposite of what it is
        if (playerInput.x < 0 && facingRight)
        {
            sR.flipX = true;
            facingRight = !facingRight;
        }
        else if (playerInput.x > 0 && !facingRight)
        {
            sR.flipX = false;
            facingRight = !facingRight;
        }
    }
}
