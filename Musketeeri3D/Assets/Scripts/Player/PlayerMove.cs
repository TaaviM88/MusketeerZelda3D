using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerEnumManager enums;
    CharacterController controller;
    Collision coll;
    PlayerAnimator anime;
    public int side = 1;

[   Header("Parameters")]
    public float moveSpeed = 7f;
    public float jumpForce = 14;
    public float maxVelocity = 14;
    public float gravity = -9.81f;
    public float pushForce = 5f;
    //poistaa mäissä pomputtelun
    public float antiBumpFactor = 0.75f;
    public Transform princeBody;

    public int inputBufferCounter = 0;
    public int inputBufferMax = 10;
    bool canMove = true;
    bool jumping = false;
    float horizontalX;
    float verticalY;
    Vector3 moveDirection;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        enums = GetComponent<PlayerEnumManager>();
        coll = GetComponent<Collision>();
        anime = GetComponent<PlayerAnimator>();
        if(side == 1)
        {
            enums.lookDir = PlayerLookDirection.Right;
        }
        else
        {
            enums.lookDir = PlayerLookDirection.Left;
        }
    }

    // Update is called once per frame
    void Update()
    {

        CheckGround();
        //CheckWall();
        

        horizontalX = Input.GetAxis("Horizontal");
        verticalY = Input.GetAxis("Vertical");

        Move();
        
        if(Input.GetButtonDown("Jump"))
        {
            CheckJump();
            inputBufferCounter = 0;
        }

        FallDown();

        velocity.y = Mathf.Clamp(velocity.y, -maxVelocity, maxVelocity);

        //anime.animenator.SetFloat("VerticalVelocity", velocity.y);
    }

    private bool CheckWall()
    {
        if(enums.lookDir == PlayerLookDirection.Right)
        {
            if (coll.onRightWall)
            {
                //Do the push
                return coll.onRightWall;
            } 
        }
        else
        {
            if (coll.onLeftWall)
            {
                //Do the push
                return coll.onLeftWall;
            }
        }

        anime.animenator.SetBool("Pushing", false);
        enums.actionState = PlayerActionState.Idle;
        return false;
    }

    private void CheckGround()
    {
        anime.animenator.SetBool("OnGround", coll.onGround);

        if(coll.onGround)
        {
            jumping = false;

            if(velocity.y < 0)
            {
                velocity.y = 0;
            }
        }
        //jump buffer
        else if(!coll.onGround &&  velocity.y < 0 && inputBufferCounter < inputBufferMax)
        {
            
            inputBufferCounter++;
            CheckJump();
        }
    }

    private void CheckJump()
    {
        if(!coll.onGround)
        {
            jumping = true;
            return;
        }

        DoJump();
    }

    private void DoJump()
    {
        //Animaatioon hyppy
        anime.animenator.SetTrigger("Jump");
        velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);
        //anime.animenator.SetFloat("VerticalVelocity", velocity.y);
        inputBufferCounter = 0;
    }

    private void Move()
    {
        moveDirection = (transform.right * horizontalX + transform.up * -antiBumpFactor);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if(moveDirection.x > 0)
        {
            if(side != 1)
            {
                side = 1;
                enums.lookDir = PlayerLookDirection.Right;
                FlipCharacter(side);
            }

        }

        if(moveDirection.x < 0)
        {
            if(side != -1)
            {
                side = -1;
                enums.lookDir = PlayerLookDirection.Left;
                FlipCharacter(side);
            }
        }
        enums.actionState = PlayerActionState.Walk;
        //HUOM! Movedirection.y ehkä väärin.
        anime.SetInputAxis(horizontalX, verticalY, velocity.y);

    }

    private void FallDown()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void FlipCharacter(int lookSide)
    {
        bool state = (side == 1) ? false : true;

        if(state)
        {
            princeBody.localRotation = Quaternion.Euler(0, 270, 0f);
        }
        else
        {
            princeBody.localRotation = Quaternion.Euler(0, 90, 0f);
        }

        anime.animenator.SetTrigger("Flip");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(CheckWall() && hit.collider.tag == "Movable")
        {
            Debug.Log(hit.collider.gameObject.name);
            Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();

            if(rb != null)
            {
                anime.animenator.SetBool("Pushing", true);
                enums.actionState = PlayerActionState.Pushing;
                if (enums.lookDir == PlayerLookDirection.Right)
                {
                    rb.AddForce((new Vector3( moveDirection.x * pushForce,0,0)));
                }
               
                if(enums.lookDir == PlayerLookDirection.Left)
                {
                    rb.AddForce((new Vector3(moveDirection.x * pushForce, 0, 0)));
                }
             
            }
        }
    }
}
