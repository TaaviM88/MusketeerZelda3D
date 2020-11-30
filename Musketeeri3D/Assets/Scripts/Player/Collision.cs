using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer, enemyLayer, playerLayer;
    [Space]
    public bool onGround, onWall, onRightWall, onLeftWall, onCeiling, stabHit;
    public int wallSide;
    [Header("Transforms")]
    
    public Transform groundCheck, ceilingCheck;
    public float groundRadius = 0.1f;
    public float ceilingRadius = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we are at ground
        onGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        Debug.Log(onGround);
        onCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingRadius, groundLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Ground check
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingRadius);
    }
}
