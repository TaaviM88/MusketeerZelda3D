using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using System;

public class ThrowWeaponSkill : MonoBehaviour
{
    PlayerAnimator anime;
    PlayerMove move;
    Rigidbody weaponRb;
    ThrowWeapon throwWeapon;
    PlayerEnumManager enums;
    Collision coll;
    private float returnTime;

    Vector3 origLockPos;
    Vector3 origLockRot;
    Vector3 pullPosition;
    [Header("Public References")]
    public Transform throwWeaponObj;
    public Transform hand;
    public Transform spine;
    public Transform curvePoint;
    [Space]
    [Header("Parameters")]
    public float throwPower = 30;
    public float cameraZoomOffset = 0.3f;
    [Space]
    [Header("Bools")]
    //HUOM! Rakenna nää PlayerEnumManagerin sisään
    public bool walking = true;
    public bool aiming = false;
    public bool hasWeapon = true;
    public bool pulling = false;

    //tähtäys kulma
    float aimRotationValue;
    Vector3 origScale;
    //koriste jutskat
    //[Space]
    //[Header("Particles and Trails")]
    //public ParticleSystem glowParticle;
    //public ParticleSystem catchParticle;
    //public ParticleSystem trailParticle;
    //public TrailRenderer trailRenderer;

    //tähtäin
    //[Space]
    //[Header("UI")]
    //public Image reticle;

    //[Space]
    ////Cinemachine Shake
    //public CinemachineFreeLook virtualCamera;


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<PlayerAnimator>();
        move = GetComponent<PlayerMove>();
        enums = GetComponent<PlayerEnumManager>();
        weaponRb = throwWeaponObj.GetComponent<Rigidbody>();
        throwWeapon = throwWeaponObj.GetComponent<ThrowWeapon>();
        coll = GetComponent<Collision>();
        origLockPos = throwWeaponObj.localPosition;
        origLockRot = throwWeaponObj.localEulerAngles;
        origScale = throwWeaponObj.localScale;
        enums.throwState = PlayerThrowState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enums.throwState)
        {
            case PlayerThrowState.Idle:
                move.canMove = true;
                pulling = false;
                aiming = false;
                break;
            case PlayerThrowState.Throwing:
                move.canMove = false;
                break;
            default:
                break;
        }

        HasWeapon();
        PullWeapon();
        Aim();

        //Debug.Log($"aim rotation: {aimRotationValue}");
    }

    private void Aim()
    {
       if(aiming)
        {
            AimArrow();
        }
       else
        {
            //pistä tähtäys pois päältä

     
        }
    }

    private void AimArrow()
    {
        //pistä tähtäys animaatio

        //Katsotaan vasemmalle
        if(enums.lookDir == PlayerLookDirection.Left)
        {
            // arrow.GetComponent<Rigidbody2D>().rotation = 180;
            aimRotationValue = 180;
            Vector3 negativeMove =new Vector3(move.horizontalX, move.verticalY);
            negativeMove.x = negativeMove.x * -1;
            float angle = Vector3.Angle(negativeMove, Vector3.right);

            if(move.verticalY >= 0)
            {
                // arrow.GetComponent<Rigidbody2D>().rotation = 180;
                aimRotationValue += -angle;
            }
            else
            {
                // arrow.GetComponent<Rigidbody2D>().rotation
                aimRotationValue += angle;
            }
        }
        //katsotaan oikealle
        else
        {
            //arrow.GetComponent<Rigidbody2D>().rotation = 0;
            aimRotationValue = 0;
            Vector3 moveDir= new Vector3(move.horizontalX, move.verticalY);
            float angle = Vector3.Angle(moveDir, Vector3.right);
            if (move.verticalY >= 0)
            {
                //arrow.GetComponent<Rigidbody2D>().rotation += angle;
                aimRotationValue += angle;
            }
            else
            {
                //arrow.GetComponent<Rigidbody2D>().rotation += -angle;
                aimRotationValue += -angle;
            }
        }
    }

    private void PullWeapon()
    {
        if (pulling)
        {
            if(returnTime < 1)
            {
                throwWeaponObj.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                WeaponCatch();
            }
        }
    }

    private void WeaponCatch()
    {
        returnTime = 0;
        pulling = false;
        throwWeaponObj.parent = hand;
        throwWeapon.activated = false;
        throwWeaponObj.localEulerAngles = origLockRot;
        throwWeaponObj.localPosition = origLockPos;
        throwWeaponObj.localScale = origScale;
        hasWeapon = true;
        enums.throwState = PlayerThrowState.Idle;
    }

    private Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    private void HasWeapon()
    {
        if (hasWeapon)
        {
            //Start aim
            if(Input.GetButtonDown("Fire1") && coll.onGround)
            {
                enums.throwState = PlayerThrowState.Throwing;

                //PlayerManager.Instance.canChangeAttackMode = false;

                //Freeze player movement;
                move.canMove = false;

                aiming = true;
               //activate weapon if not active
            }

            //Throw Axe
            if(Input.GetButtonUp("Fire1"))
            {
                aiming = false;
                WeaponThrow();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1")&& throwWeapon.canPulled)
            {
                WeaponStartPull();
            }

            if(enums.throwState != PlayerThrowState.Throwing)
            {
                enums.throwState = PlayerThrowState.Throwing;
            }
        }
    }

    private void WeaponStartPull()
    {
        pullPosition = throwWeaponObj.position;
        weaponRb.Sleep();
        weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        weaponRb.isKinematic = true;
        throwWeaponObj.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        throwWeaponObj.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        throwWeapon.activated = true;
        pulling = true;
    }

    private void WeaponThrow()
    {
        hasWeapon = false;
        throwWeapon.activated = true;

        weaponRb.isKinematic = false;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        throwWeaponObj.parent = null;
        throwWeaponObj.localScale = origScale * 100;
        throwWeaponObj.eulerAngles = new Vector3(0, 0, aimRotationValue);
        throwWeaponObj.transform.position += transform.right / 5;

        if(enums.lookDir == PlayerLookDirection.Right)
        {
            weaponRb.AddForce(throwWeaponObj.transform.right * throwPower, ForceMode.Impulse);
        }
        else
        {
           // throwWeaponObj.eulerAngles = new Vector3(0, 0, -aimRotationValue);
            Debug.Log("VITTU");
            weaponRb.AddForce(throwWeaponObj.transform.right * throwPower, ForceMode.Impulse);
        }
    }

}
