using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PiercingBullet : Shootable
{
    // Start is called before the first frame update
    public override void HitScan()
    {
        Vector3 displacement = transform.position - prevPos;
        var hits = Physics.SphereCastAll(prevPos, Radius, displacement.normalized, displacement.magnitude, hitMask);
        var hitLimit = hits.Any(x => x.collider.gameObject.CompareTag("Map"));
        var maxDistance = hitLimit ? hits.Where(x => x.collider.gameObject.CompareTag("Map")).Min(x => x.distance) : Mathf.Infinity;
        foreach(RaycastHit hit in hits)
        {
            if(hit.distance <= maxDistance) { 
                Hit(hit);
            }
        }

        if (hitLimit) {
            state = States.Hit;
        }
    }
}
