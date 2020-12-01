using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animenator;
    PlayerMove move;
    Collision coll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInputAxis(float x, float y, float yVel)
    {
        animenator.SetFloat("HorizontalAxis", x);
        animenator.SetFloat("VerticalAxis", y);
        animenator.SetFloat("VerticalVelocity", yVel);
    }

    public void Flip()
    {
        //Do flippaus animaatio kun aika koittaa
    }
}
