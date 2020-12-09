using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPush : MonoBehaviour
{
    public GameObject pushableObj = null;
    PlayerAnimator anime;
    PlayerEnumManager enums;
    PlayerMove move;
    Collision coll;
    //public float pushForce = 10f;
    public float rightDistance = 0.03f;
    public float leftDistance = 0.03f;
    public Transform rightCheck, leftCheck;
    public LayerMask pushableLayer;
    bool canPush = true;
    bool isPushing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<PlayerAnimator>();
        enums = GetComponent<PlayerEnumManager>();
        move = GetComponent<PlayerMove>();
        coll = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") &&  coll.onGround)
        {
            Grab();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            ReleaseGrab();
        }

        Push();
    }

    public void Push()
    {
        if(pushableObj != null) // if(enums.actionState == PlayerActionState.Pushing)
        {

            enums.actionState = PlayerActionState.Pushing;
            anime.animenator.SetBool("Pushing", true);
            isPushing = true;
             //_rbPushableObj.AddForce((new Vector3(move.GetMoveDirection().x * pushForce, 0, 0)));
        }

       if(!coll.onGround)
        {
            ReleaseGrab();
        }
    }

    private void ReleaseGrab()
    {
        if(pushableObj != null)
        {
            pushableObj.transform.parent = null;
            pushableObj = null;
            
        }
        isPushing = false;
        anime.animenator.SetBool("Pushing", false);
        enums.actionState = PlayerActionState.Idle;
    }

    private void Grab()
    {

        if(isPushing)
        {
            return;
        }

        //tsekataan osutaanko esineeseen mitä voi työntää
        RaycastHit hit;
        
        if(enums.lookDir == PlayerLookDirection.Right)
        {
            
            if (Physics.Raycast(rightCheck.position, Vector3.right, out hit, rightDistance, pushableLayer))
            {

                //_rbPushableObj = hit.rigidbody;
                pushableObj = hit.transform.gameObject;
                hit.transform.parent = transform;
            
            }
            else
            {
                enums.actionState = PlayerActionState.Idle;
            }

        }
        else
        {
          if (Physics.Raycast(leftCheck.position, -Vector3.right, out hit, rightDistance, pushableLayer))
            {
                // _rbPushableObj = hit.rigidbody;
                pushableObj = hit.transform.gameObject;
                hit.transform.parent = transform;
            }
            else
            {
                enums.actionState = PlayerActionState.Idle;
            }   
        }
    }

}
