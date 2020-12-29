using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public bool activated;

    public float rotationSpeed;

    LayerMask collisionLayer;

    public bool canPulled { get; set; }

    private void Start()
    {
        // muokkaa myöhemmin fiksummaksi
        canPulled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }

    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Saatana");
        if(collision.gameObject.layer == collisionLayer)
        {
            print(collision.gameObject.name);
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            activated = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Breakable"))
    //    {
    //        if (other.GetComponent<BreakBoxScript>() != null)
    //        {
    //            other.GetComponent<BreakBoxScript>().Break();
    //        }
    //    }
    //}
}
