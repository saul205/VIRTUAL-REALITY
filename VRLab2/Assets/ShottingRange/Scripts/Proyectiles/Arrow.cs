using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : ChargedProjectile
{
    // Start is called before the first frame update
    public Rigidbody body;
    public bool hit = false;
    protected override void Update()
    {
        base.Update();

        if(shot && body.velocity != Vector3.zero)
            transform.forward = body.velocity.normalized;

        if(hit && transform.parent == null)
        {
            fall = true;
            hit = false;
            body.useGravity = true;
        }
    }

    public override void Shoot(WeaponController controller)
    {
        base.Shoot(controller);

        body.useGravity = true;
        body.AddForce(transform.forward * Charge * Speed, ForceMode.Impulse);
    }
    public override void HitEffects()
    {
        shot = false;
        hit = true;
        body.velocity = Vector3.zero;
        body.useGravity = false;
        transform.position = hitPoint;
        gameObject.transform.SetParent(hitObject.transform, true);
    }
    public override void Fly()
    {
       
    }
}
