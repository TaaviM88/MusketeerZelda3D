using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    PlayerMove move;
    public float fallMultiplayer = 2.5f;
    public float lowJumpMultiplier = 2f;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        if(move.velocity.y < 0)
        {
            move.velocity += Vector3.up * move.gravity * (fallMultiplayer) * Time.deltaTime;
        }
        else if (move.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            move.velocity += Vector3.up * move.gravity * (lowJumpMultiplier ) * Time.deltaTime;
        }
    }
}
