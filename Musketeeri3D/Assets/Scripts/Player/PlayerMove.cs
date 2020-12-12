using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    WallRun wallRun;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        enums = GetComponent<PlayerEnumManager>();
        coll = GetComponent<Collision>();
        anime = GetComponent<PlayerAnimator>();
        wallRun = GetComponent<WallRun>();
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
        if(transform.position.z != 0)
        {
           // Debug.Log("lul, melkein lipsahdettiin pois Z:lta");
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
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
        //enums.actionState = PlayerActionState.Idle;
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
                velocity = Vector3.zero;
            }
        }
        //jump buffer
        //else if(!coll.onGround &&  velocity.y < 0 && inputBufferCounter < inputBufferMax)
        //{
            
        //    inputBufferCounter++;
        //    CheckJump();
        //}
    }

    private void CheckJump()
    {
        if(!coll.onGround && !wallRun.IsWallRunning())
        {
            jumping = true;
            return;
        }

        DoJump();
    }

    private void DoJump()
    {
        if(coll.onGround && !wallRun.IsWallRunning())
        {
            //Animaatioon hyppy
            anime.animenator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);
            //anime.animenator.SetFloat("VerticalVelocity", velocity.y);
            inputBufferCounter = 0;
        }
        
        if(wallRun.IsWallRunning())
        {
            //Animaatioon hyppy
            //anime.animenator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            //anime.animenator.SetFloat("VerticalVelocity", velocity.y);
            inputBufferCounter = 0;
        }

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
        //enums.actionState = PlayerActionState.Walk;
        //HUOM! Movedirection.y ehkä väärin.
        anime.SetInputAxis(horizontalX, verticalY, velocity.y);

    }

    private void FallDown()
    {
        if(!wallRun.IsWallRunning())
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            controller.Move(velocity * Time.deltaTime);
        }
      
    }

    public void FlipCharacter(int lookSide)
    {
        if (enums.actionState != PlayerActionState.Pushing)
        {
            bool state = (lookSide == 1) ? false : true;

            if (state)
            {
                princeBody.localRotation = Quaternion.Euler(princeBody.localEulerAngles.x, 270, princeBody.localEulerAngles.z);
                enums.lookDir = PlayerLookDirection.Left;
                anime.animenator.SetFloat("LookDirection", -1);
            }
            else
            {
                princeBody.localRotation = Quaternion.Euler(princeBody.localEulerAngles.x, 90, princeBody.localEulerAngles.z);
                enums.lookDir = PlayerLookDirection.Right;
                anime.animenator.SetFloat("LookDirection", 1);
            }

            anime.animenator.SetTrigger("Flip");

        }
           
    }

    public Transform GetPrinceBody()
    {
        return princeBody;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}
