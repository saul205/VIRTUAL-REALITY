using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Owner {get;set;}
    public Shootable bulletPrefab;
    public Transform bulletSpawn;
    public float Charge = 0;

    public float rof = 120;
    protected float lastShot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
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
        if (ReadyToShoot())
        {
            HandleShoot();
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
}
