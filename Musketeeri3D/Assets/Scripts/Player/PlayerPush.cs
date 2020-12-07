using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    Rigidbody _rbPushableObj = null;
    PlayerAnimator anime;
    PlayerEnumManager enums;
    PlayerMove move;
    public float pushForce = 10f;
    public float rightRadius = 0.03f;
    public float leftRadius = 0.03f;
    public Transform rightCheck, leftCheck;
    public LayerMask pushableLayer;
    bool canPush = true;
    
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<PlayerAnimator>();
        enums = GetComponent<PlayerEnumManager>();
        move = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && canPush)
        {
            Grab();
        }

        if(Input.GetButtonUp("Fire1") && canPush)
        {
            ReleaseGrab();
        }

        Push();
        Debug.Log(_rbPushableObj);
    }

    public void Push()
    {
        if(_rbPushableObj != null) // if(enums.actionState == PlayerActionState.Pushing)
        {
            
             _rbPushableObj.AddForce((new Vector3(move.GetMoveDirection().x * pushForce, 0, 0)));
        }
    }

    private void ReleaseGrab()
    {
        _rbPushableObj = null;
        enums.actionState = PlayerActionState.Idle;
    }

    private void Grab()
    {
        //tsekataan osutaanko esineeseen mitä voi työntää
        RaycastHit hit;
        
        if(enums.lookDir == PlayerLookDirection.Right)
        {
            if (Physics.SphereCast(rightCheck.position, rightRadius, Vector3.zero, out hit, pushableLayer))
            {
                _rbPushableObj = hit.rigidbody;

                if (_rbPushableObj != null)
                {
                    enums.actionState = PlayerActionState.Pushing;
                }
                else
                {
                    enums.actionState = PlayerActionState.Idle;
                }
            }
            else
            {
                enums.actionState = PlayerActionState.Idle;
            }

        }
        else
        {
            if (Physics.SphereCast(leftCheck.position, leftRadius, Vector3.zero, out hit, pushableLayer))
            {
                _rbPushableObj = hit.rigidbody;

                if (_rbPushableObj != null)
                {
                    enums.actionState = PlayerActionState.Pushing;
                }
                else
                {
                    enums.actionState = PlayerActionState.Idle;
                }
            }
            else
            {
                enums.actionState = PlayerActionState.Idle;
            }
            
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Ground check


        Gizmos.DrawWireSphere(rightCheck.position, rightRadius);
        Gizmos.DrawWireSphere(leftCheck.position, leftRadius);
    }
}
