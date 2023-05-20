using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    protected bool reloading = false;
    protected float reloadTime = 3.5f;
    protected float reloadStart = 3;

    public float recoilValue = 0;
    public float recoilSpeed = 0f;
    public float recoverySpeed = 0f;

    public float RecoilMult = 0f;
    public float MinRecoilMult = 0f;
    public float MaxRecoilMult = 2f;

    public AudioSource shootSound;
    // Start is called before the first frame update
    void Awake()
    {
        AmmoCount = StartingAmmo;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        PlayerWeaponManager wepManager = Owner.GetComponent<PlayerWeaponManager>();
        if(wepManager != null)
        {
            wepManager.targetRotation = Vector3.Lerp(wepManager.targetRotation, Vector3.zero, recoverySpeed * Time.deltaTime);
            wepManager.currentRotation = Vector3.Slerp(wepManager.currentRotation, wepManager.targetRotation, recoilSpeed * Time.deltaTime);
            wepManager.aimSpot.localEulerAngles = wepManager.currentRotation;

            if (RoF())
            {
                RecoilMult = MinRecoilMult; // Mathf.Max(RecoilMult - 0.2f, 0);
            }

            if (reloading)
            {
                if(Time.time >= reloadStart + reloadTime)
                {
                    Reload(wepManager);
                }
            }
            else
            {
                if (AmmoCount == 0 && wepManager.AutoReload)
                    {
                        StartReload();
                    }
                    else if(reload)
                    {
                        StartReload();
                    }
                }
            }
    }

    public void Recoil()
    {
        PlayerWeaponManager wepManager = Owner.GetComponent<PlayerWeaponManager>();
        wepManager.targetRotation = wepManager.currentRotation + Vector3.left * Mathf.Pow(recoilValue, RecoilMult);
        RecoilMult = Mathf.Min(RecoilMult + 0.2f, MaxRecoilMult);
    }

    protected virtual void HandleShoot()
    {
        shootSound.Play();
        var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.Shoot(this);

        lastShot = Time.time;
        Recoil();
    }

    protected bool RoF()
    {
        return Time.time > lastShot + 60 / rof;
    }

    protected bool ReadyToShoot()
    {
        return RoF() && !reloading;
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

    protected virtual void StartReload()
    {
        if(AmmoCount < MaxAmmo)
        {
            reloadStart = Time.time;
            reloading = true;
            this.gameObject.GetComponent<Animator>().SetTrigger("reload");
            reloadStart = Time.time;
        }
        else
        {
            reload = false;
        }
            
    }

    public void Reload(PlayerWeaponManager wepManager)
    {
        var reloadAmount = wepManager.AmmoManager.TakeAmmo(bulletPrefab, MaxAmmo - AmmoCount);
        if(reloadAmount > 0)
        {
            AmmoCount += reloadAmount;
        }

        reloading = false;
        reload = false;
    }

    public void ResetWeapon()
    {
        transform.localEulerAngles = Vector3.zero;
        RecoilMult = 0;
        reloading = false;
        reload = false;
    }
}
