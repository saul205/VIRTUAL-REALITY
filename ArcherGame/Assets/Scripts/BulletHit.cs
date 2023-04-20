using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour 
{
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        else if (coll.gameObject.CompareTag("Target"))
        {
            coll.gameObject.GetComponent<IDamageable>().Hit(gameObject.GetComponent<IShootable>().damage);
            Destroy(gameObject);
        }
    }
}
