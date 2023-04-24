using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PiercingBullet : Shootable
{
    public override void HitScan()
    {
        Vector3 displacement = transform.position - prevPos;
        var hits = Physics.SphereCastAll(prevPos, Radius, displacement.normalized, displacement.magnitude, hitMask, QueryTriggerInteraction.Ignore);

        float maxDist = float.PositiveInfinity;
        foreach(var hit in hits)
        {
            if (blockMask == (blockMask | (1 << hit.collider.gameObject.layer)))
            {
                if(hit.distance < maxDist)
                    maxDist = hit.distance;
            }
        }


        foreach(var hit in hits) {

            if(hit.distance <= maxDist)
                Hit(hit);
        }

        if (maxDist != float.PositiveInfinity)
        {
            state = States.Hit;
        }
    }
}
