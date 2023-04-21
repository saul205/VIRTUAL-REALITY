using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetCollisions : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && damageable.gameObject.CompareTag("Player")){
            damageable.Hit(10);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && damageable.gameObject.CompareTag("Player"))
        {
            damageable.Hit(10);
        }
    }
}
