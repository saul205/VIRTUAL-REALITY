using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public GameObject Owner {get;set;}
    public Shootable bulletPrefab;
    public Transform bulletSpawn;
    public float Charge = 0;

    public float rof = 120;
    protected float lastShot;

    public int MaxAmmo = 50;
    public int StartingAmmo = 30;

    protected int AmmoCount;
    protected bool reload = false;
    // Start is called before the first frame update
    void Start()
    {
        AmmoCount = StartingAmmo;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        PlayerWeaponManager wepManager = Owner.GetComponent<PlayerWeaponManager>();
        if(wepManager != null)
        {
            if (AmmoCount == 0 && wepManager.AutoReload)
            {
                Reload(wepManager);
            }
            else if(reload)
            {
                Reload(wepManager);
            }
        }
    }

    protected virtual void HandleShoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.Shoot(this);

        lastShot = Time.time;
    }

    protected bool ReadyToShoot()
    {
        return Time.time > lastShot + 60 / rof;
    }

    public bool TryShoot()
    {
        if (ReadyToShoot() && AmmoCount > 0)
        {
            HandleShoot();
            ReduceAmmo();
            return true;
        }

        return false;
    }

    public virtual bool Shoot(bool hold, bool release)
    {
        if (hold)
        {
            return TryShoot();
        }

        return false;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    public void ReduceAmmo()
    {
        AmmoCount--;
    }

    public int GetAmmoCount()
    {
        return AmmoCount;
    }

    public void SetReload()
    {
        reload = true;
    }

    public void Reload(PlayerWeaponManager wepManager)
    {
        var reloadAmount = wepManager.AmmoManager.TakeAmmo(bulletPrefab, MaxAmmo - AmmoCount);
        if(reloadAmount > 0)
        {
            AmmoCount += reloadAmount;
        }

        reload = false;
    }
}
