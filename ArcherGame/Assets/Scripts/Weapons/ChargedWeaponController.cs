using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedWeaponController : WeaponController
{
    #region Charged Parameters

    public float TimeToFullyCharge = 5f;

    protected float ChargePerSecond = 5;
    protected bool IsCharging = false;

    protected bool canceled = false;

    #endregion
    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        UpdateCharge();
    }

    protected void UpdateCharge()
    {
        if(IsCharging && Charge < 1f)
        {
            Charge += (1f / TimeToFullyCharge) * Time.deltaTime;
        }
    }

    public override bool Shoot(bool press, bool hold, bool release, bool cancel)
    {
        if (press)
        {
            canceled = false;
        }

        if (cancel)
        {
            OnCancelCharge();
        }
        else if (hold && !canceled)
        {
            TryChargeWeapon();
        }
        else if (release)
        {
            return TryRelease();
        }

        return false;
    }

    public virtual bool TryChargeWeapon()
    {
        if(!IsCharging && ReadyToShoot() && AmmoCount > 0)
        {
            IsCharging = true;
            return true;
        }

        return false;
    }

    public virtual bool TryRelease()
    {
        if (!IsCharging)
            return false;

        HandleShoot();
        ReduceAmmo();

        IsCharging = false;
        Charge = 0;

        return true;
    }

    public virtual void OnCancelCharge()
    {
        IsCharging = false;
        Charge = 0;
        canceled = true;
    }

    protected override void StartReload()
    {
        if(!IsCharging)
            base.StartReload();
    }
}
