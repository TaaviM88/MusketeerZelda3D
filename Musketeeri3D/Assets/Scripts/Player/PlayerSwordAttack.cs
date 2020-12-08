using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttack : MonoBehaviour
{
    public int attackPower = 1;
    public float attackCooldownMultiplayer = 2;
    
    bool canAttack = true;
    bool animeOn = false;
    float timeBTWAttack = 0;
    float startTimeBtwAttack;
    ItakeDamage<int> enemyToDamage;
    int comboAttackCount = 0;

    Collision coll;
    PlayerEnumManager enums;
    PlayerMove move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
