using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : ChargedWeaponController
{
    public float MaxPullback = .4f;
    private Shootable bullet;
    private float PrevCharge = 0f;

    public LineRenderer bowString;
    private Vector3 stringPosition;

    public void Start()
    {
        stringPosition = bowString.GetPosition(1);
    }
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
        float magnitude = MaxPullback * (Charge - PrevCharge);
        bullet.transform.localPosition -= Vector3.forward * magnitude;
        bowString.SetPosition(1, bowString.GetPosition(1) + Vector3.right * magnitude * 2);
        PrevCharge = Charge;
    }

    public override bool TryRelease()
    {
        bool result = base.TryRelease();
        if (result)
        {
            PrevCharge = 0;
            bowString.SetPosition(1, stringPosition);
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
            bowString.SetPosition(1, stringPosition);
        }
    }
}
