using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedProjectile : Shootable
{
    public float minSpeed = 10;
    public float maxSpeed = 50;

    public int minDamage = 10;
    public int maxDamage = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Shoot(WeaponController controller)
    {
        base.Shoot(controller);

        Speed = Mathf.Lerp(minSpeed, maxSpeed, Charge);
        damage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, Charge));
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
    }
}
