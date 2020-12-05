using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer, enemyLayer, playerLayer, pushableLayer;
    [Space]
    public bool onGround, onWall, onRightWall, onLeftWall, onCeiling, stabHit;
    public int wallSide;
    [Header("Transforms")]
    
    public Transform groundCheck, ceilingCheck, rightCheck, leftCheck;
    [Space]
    public float groundRadius = 0.1f;
    public float ceilingRadius = 0.1f;
    [Space]
    public float rightRadius = 0.1f;
    public float leftRadius = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we are at ground
        onGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        onCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingRadius, groundLayer);

        //tsekataan osutaanko seinään. 
        onRightWall =  Physics.CheckSphere(rightCheck.position, rightRadius, pushableLayer);
        onLeftWall = Physics.CheckSphere(leftCheck.position, leftRadius, pushableLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Ground check
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingRadius);

        Gizmos.DrawWireSphere(rightCheck.position, rightRadius);
        Gizmos.DrawWireSphere(leftCheck.position, leftRadius);
    }
}
