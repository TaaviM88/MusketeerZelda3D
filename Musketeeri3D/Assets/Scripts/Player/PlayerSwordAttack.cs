using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerSwordAttack : MonoBehaviour
{
    public int attackPower = 1;
    public float attackCooldownMultiplayer = 2;
    public float attackRange = 3f;
    public WeaponScript weapon;

    //public Vector3 slashBoxSize = new Vector3(0, 0, 0);
    //public Transform rightStandAttackNode, leftStandAttackNode;
    
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

        weapon?.onHit.AddListener((target) => DoDamage(target));
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        //Trigger animation:Done
        
        anime.animenator.SetTrigger("SwordSlash");
        anime.animenator.SetFloat("AttackCombo", comboAttackCount);
        
        if(comboAttackCount  >= 2)
        {
            comboAttackCount = 0;
        }
        else
        {
            comboAttackCount++;
        }
       
        //check if we hit anything

       //If we hit do damage: Done

       //if player attacks second time do second attack
       //check again if hit anything
       //Do damage

       //Check if player attack third time -> repeate second part
       //If player hit fourth time -> same as the first
    }


    //Do the damage
    public void DoDamage(Transform target)
    {
        //damageable?.Damage(attackPower);
        //Debug.Log($"Attacing enemy:");
        
        target?.GetComponent<ItakeDamage<int>>().Damage(attackPower);
    }


    public void CheckSwordAttackCollision()
    {
        //RaycastHit[] hit;

        //if(enums.lookDir == PlayerLookDirection.Right)
        //{
        //    if(Physics.BoxCast(rightStandAttackNode.position, slashBoxSize, Vector3.right, out hit,Quaternion.LookRotation(transform.position,Vector3.forward)))
        //    {
        //        for (int i = 0; i < length; i++)
        //        {

        //        }
        //        hit[].collider.gameObject?.GetComponent<ItakeDamage<int>>();
        //    }
        //}
    }

  
}
