using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class CollisionEvent : UnityEvent<Transform> { }
public class WeaponScript : MonoBehaviour
{
    public CollisionEvent onHit;

    private void OnTriggerEnter(Collider other)
    {

        //ItakeDamage<int> target = other.GetComponent(typeof(ItakeDamage<int>)) as ItakeDamage<int>;

        ItakeDamage<int> target = other.gameObject.GetComponent<ItakeDamage<int>>();
        Debug.Log(other.gameObject.name);
       // Debug.Log(target);

        if (target != null)
        {
            Debug.Log("lul");
            onHit.Invoke(other.transform);
        }
        
    }
}
