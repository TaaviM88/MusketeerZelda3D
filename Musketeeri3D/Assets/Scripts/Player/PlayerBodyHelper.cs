using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyHelper : MonoBehaviour
{
    PlayerSwordAttack swordAttack;
    // Start is called before the first frame update
    void Start()
    {
        swordAttack = GetComponentInParent<PlayerSwordAttack>();
    }

    public void EndSwordAttackCooldown()
    {
        if(swordAttack!= null)
        {
            swordAttack.EndSwordAttackCooldown();
        }
      else
        {
            Debug.Log("Ei löydy saatana");
        }
    }
}
