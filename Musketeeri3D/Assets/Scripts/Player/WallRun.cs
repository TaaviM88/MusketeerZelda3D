using System.Linq;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Rendering;
using DG.Tweening;
public class WallRun : MonoBehaviour
{
    public LayerMask runAlongWallLayer;
    public float wallMaxDistance = 1;
    public float wallSpeedMultiplier = 12;
    public float minimumHeight = 0.7f;
    public float maxAngleRoll = 30;
    [Range(0f, 1f)]
    public float normalizedAngleThreshold = 0.1f;
    public float jumpDuration = 0.25f;
    public float wallBouncing = 3;
    public float cameraTransitionDuration = 1;
    public float wallGravityDownForce = 20;
    public bool useSprint;

    PlayerMove move;
    Collision coll;
    PlayerEnumManager enums;
    BetterJumping betterjumping;
    PlayerAnimator anime;
    Vector3[] directions;
    RaycastHit[] hits;

    bool isWallRunning = false;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    float elapsedTimeSinceJump = 0;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetach = 0;
    bool jumping;
    bool canRotate = true;
    bool isPlayerGrounded() => coll.onGround;
    public bool IsWallRunning() => isWallRunning;

    Camera mainCamera;
    bool CanWallRun()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetButton("Fire3");
        isSprinting = !useSprint ? true : isSprinting;
        return !isPlayerGrounded() && verticalAxis > 0 && VerticalCheck() && isSprinting;
    }

    private bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMove>();
        coll = GetComponent<Collision>();
        enums = GetComponent<PlayerEnumManager>();
        betterjumping = GetComponent<BetterJumping>();
        anime = GetComponent<PlayerAnimator>();
        mainCamera = Camera.main;
        directions = new Vector3[]
        {
            //Vector3.right,
            //Vector3.right+Vector3.forward,
            Vector3.forward,
            //Vector3.left + Vector3.forward,
            //Vector3.left,
            Vector3.back
        };

        //if (Time.timeScale != 1)
        //{
        //    SetTimeScale(1);
        //}
    }

    // Update is called once per frame
    void LateUpdate()
    {

        isWallRunning = false;

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        if (CanAttach())
        {
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], wallMaxDistance,runAlongWallLayer);
                if (hits[i].collider != null)
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.red);
                }
            }
            if (CanWallRun())
            {
                hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if (hits.Length > 0)
                {
                    OnWall(hits[0]);
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }

            }
         

        }
        if (isWallRunning)
        {
            elapsedTimeSinceWallDetach = 0;
           
           if (elapsedTimeSinceWallAttach == 0)
            {
                StartWallRun();
            }
            elapsedTimeSinceWallAttach += Time.deltaTime;

            move.velocity += Vector3.down * wallGravityDownForce * Time.deltaTime;
        }
        else
        {
            elapsedTimeSinceWallAttach = 0;
            if (elapsedTimeSinceWallDetach == 0)
            {
                EndWallRun();
            }
            elapsedTimeSinceWallDetach += Time.deltaTime;
        }

        
    }

    private void EndWallRun()
    {
        betterjumping.enabled = true;
        move.velocity = new Vector3(0, move.velocity.y, 0);
        anime.animenator.SetBool("IsWallRun",isWallRunning);
        RotateCharacter();
    }

    private void StartWallRun()
    {
        betterjumping.enabled = false;
        anime.animenator.SetBool("IsWallRun", isWallRunning);
        RotateCharacter();
    }

    

    private void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 alongWall = transform.TransformDirection(Vector3.forward);
            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);
            move.velocity = alongWall  * wallSpeedMultiplier;
            isWallRunning = true;
        }

    }

    private bool CanAttach()
    {
        if (jumping)
        {
            elapsedTimeSinceJump += Time.deltaTime;
            if (elapsedTimeSinceJump > jumpDuration)
            {
                elapsedTimeSinceJump = 0;
                jumping = false;
            }
            return false;
        }

        return true;
    }


    public void RotateCharacter()
    {
        Transform body = move.GetPrinceBody();
        if(isWallRunning)
        {
            if (enums.lookDir == PlayerLookDirection.Right)
            {
                //Mathf.Lerp(body.localEulerAngles.z, -25, elapsedTimeSinceWallAttach / cameraTransitionDuration);
                body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, -35);
                //body.DOLocalRotate(Vector3.forward * - 25, 50);
            }
            else
            {
                // Mathf.Lerp(body.localEulerAngles.z, 25, elapsedTimeSinceWallAttach / cameraTransitionDuration);
                body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, 35);
                //body.DOLocalRotate(Vector3.forward *25, 50);
            }
        }
        else
        {
            //Mathf.Lerp(body.localEulerAngles.z, 0, elapsedTimeSinceWallDetach / cameraTransitionDuration);
            body.localEulerAngles = new Vector3(body.localEulerAngles.x, body.localEulerAngles.y, 0);
            //body.DOLocalRotate(Vector3.forward * 0, 50f);
        }

        //StartCoroutine(RotateCoolDown());
    } 

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = mainCamera.transform.eulerAngles.z;
        float targetAngle = 0;
        if (dir != 0)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }
        return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(elapsedTimeSinceWallDetach, elapsedTimeSinceWallDetach) / cameraTransitionDuration);

    }

    private float CalculateSide()
    {
        if (isWallRunning)
        {
            Vector3 heading = lastWallPosition - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    public Vector3 GetWallJumpDirection()
    {
        if (isWallRunning)
        {
            return lastWallNormal * wallBouncing + Vector3.up;
        }
        return Vector3.zero;
    }

    IEnumerator RotateCoolDown()
    {
            canRotate = false;
            yield return new WaitForSeconds(0.1f);
            canRotate = true;
    }
}
