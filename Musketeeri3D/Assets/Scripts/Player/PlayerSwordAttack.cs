using System;
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
    PlayerAnimator anime;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        anime = GetComponent<PlayerAnimator>();
        move = GetComponent<PlayerMove>();
        enums = GetComponent<PlayerEnumManager>();

        startTimeBtwAttack = attackCooldownMultiplayer;
        timeBTWAttack = attackCooldownMultiplayer;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        //Trigger animation:Done
        
        anime.animenator.SetTrigger("SwordSlash");
       
        //check if we hit anything

       //If we hit do damage: Done

       //if player attacks second time do second attack
       //check again if hit anything
       //Do damage

       //Check if player attack third time -> repeate second part
       //If player hit fourth time -> same as the first
    }


    //Do the damage
    public void DoDamage(ItakeDamage<int> damageable)
    {
        damageable?.Damage(attackPower);
        Debug.Log($"Attacing enemy:");
    }
}
