using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerEnumManager enums;
    CharacterController controller;
    Collision coll;
    public int side = 1;

[   Header("Parameters")]
    public float moveSpeed = 7f;
    public float jumpForce = 14;
    public float maxVelocity = 14;
    public float gravity = -9.81f;

    public Transform princeBody;
    bool canMove = true;
    bool jumping = false;
    float horizontalX;
    float verticalY;
    Vector3 moveDirection;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        enums = GetComponent<PlayerEnumManager>();
        coll = GetComponent<Collision>();

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

        horizontalX = Input.GetAxis("Horizontal");
        verticalY = Input.GetAxis("Vertical");

        Move();
        
        if(Input.GetButtonDown("Jump"))
        {
            CheckJump();
        }

        FallDown();


    }

    private void CheckGround()
    {
        if(coll.onGround)
        {
            jumping = false;
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
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    private void Move()
    {
        moveDirection = transform.right * horizontalX * moveSpeed * Time.deltaTime;
        controller.Move(moveDirection);

        if(moveDirection.x > 0)
        {
            side = 1;
            enums.lookDir = PlayerLookDirection.Right;
            FlipCharacter(side);
        }

        if(moveDirection.x < 0)
        {
            side = -1;
            enums.lookDir = PlayerLookDirection.Left;
            FlipCharacter(side);
        }
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
            princeBody.localRotation = Quaternion.Euler(0, 220, 0f);
        }
        else
        {
            princeBody.localRotation = Quaternion.Euler(0, 90, 0f);
        }
    }
}
