using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, ItakeDamage<int>,IDie
{
    public int health = 5;
    public float iframes = 0.25f;
    bool isAlive = true;
    bool iframesOn = false;
    Color orginalColor;

    private void Start()
    {
        orginalColor = GetComponent<Renderer>().material.color;
    }
    public void Damage(int Damage)
    {
        if(!isAlive)
        {
            return;
        }

        if(iframesOn)
        {
            return;
        }

        health = Mathf.Max(health - Damage, 0);
        StartCoroutine(IframeTimer());

        Debug.Log($"{gameObject.name} took : {Damage}  damage");

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //do the effects ja animations
        DestroyObj();
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    IEnumerator IframeTimer()
    {
        iframesOn = true;
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(iframes);
        iframesOn = false;
        GetComponent<Renderer>().material.SetColor("_BaseColor", orginalColor);
    }
}
