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

    public float recoilValue = 0;
    public float recoilSpeed = 0f;
    public float recoverySpeed = 0f;

    public float RecoilMult = 0f;
    public float MinRecoilMult = 0f;
    public float MaxRecoilMult = 2f;

    private Vector3 targetRotation = Vector3.zero;
    private Vector3 currentRotation = Vector3.zero;
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
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, recoverySpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, recoilSpeed * Time.deltaTime);
            wepManager.aimSpot.localEulerAngles = currentRotation;

            if (ReadyToShoot())
            {
                RecoilMult = MinRecoilMult; // Mathf.Max(RecoilMult - 0.2f, 0);
            }

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
    public void Recoil()
    {
        targetRotation = currentRotation + Vector3.left * Mathf.Pow(recoilValue, RecoilMult);
        RecoilMult = Mathf.Min(RecoilMult + 0.2f, MaxRecoilMult);
    }

    protected virtual void HandleShoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.Shoot(this);

        lastShot = Time.time;
        Recoil();
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

    public virtual bool Shoot(bool press, bool hold, bool release, bool cancel)
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
