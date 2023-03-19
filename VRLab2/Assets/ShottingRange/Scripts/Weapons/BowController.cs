using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : ChargedWeaponController
{
    public float MaxPullback = .4f;
    private Shootable bullet;
    private float PrevCharge = 0f;
    protected override void HandleShoot()
    {
        bullet.transform.SetParent(null);
        bullet.Shoot(this);

        lastShot = Time.time;
    }

    public override bool TryChargeWeapon()
    {
        bool result = base.TryChargeWeapon();
        if (result)
        {
            bullet = Instantiate(bulletPrefab, bulletSpawn);
        }
        
        return result;
    }

    protected override void Update()
    {
        base.Update();

        if (IsCharging && bullet != null) 
            UpdateArrowPosition();
    }
    
    protected virtual void UpdateArrowPosition()
    {
        bullet.transform.position -= bullet.transform.forward * MaxPullback * (Charge - PrevCharge);
        PrevCharge = Charge;
    }

    public override bool TryRelease()
    {
        bool result = base.TryRelease();
        if (result)
        {
            PrevCharge = 0;
        }

        return result;
    }
    public override void OnCancelCharge()
    {
        base.OnCancelCharge();

        if (bullet != null)
        {
            Destroy(bullet.gameObject);
            bullet = null;
            PrevCharge = 0;
        }
    }
}
