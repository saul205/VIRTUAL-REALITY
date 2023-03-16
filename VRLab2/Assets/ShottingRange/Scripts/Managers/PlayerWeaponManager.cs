using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons;
    public WeaponController[] weapons;
    public Camera cam;
    public Transform weaponSpawn;

    private bool hold = false;
    private bool release = false;
    public int weaponSlots = 2;

    public int ActiveWeaponIndex = -1;
    void Start()
    {
        weapons = new WeaponController[5];
        foreach(var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ActiveWeaponIndex >= 0)
        {
            weapons[ActiveWeaponIndex].Shoot(hold, release);
        }
    }

    public void AddWeapon(WeaponController weapon)
    {
        bool added = false;
        //Search for first free slot
        for(int i = 0; i < weaponSlots && !added; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = Instantiate(weapon, weaponSpawn);
                weapons[i].Owner = gameObject;
                weapons[i].Show(false);
                added = true;
                if(ActiveWeaponIndex < 0)
                {
                    ActiveWeaponIndex = 0;
                    SetActiveWeapon(-1);
                }
            }
        }
    }

    private void OnShoot()
    {
        release = hold;
        hold = !hold;
    }

    private void OnSwitchWeapon(InputValue value)
    {
        
        if (value.Get() != null)
        {
            SwitchWeapon(value.Get<float>() > 0);
        }
    }

    private void SwitchWeapon(bool next)
    {
        var prevIndex = ActiveWeaponIndex;
        if(ActiveWeaponIndex < 0 && weapons.Any())
        {
            ActiveWeaponIndex = 0;
        }
        else
        {
            for(int i = 0; i < weaponSlots - 1; i++)
            {
                var index = (ActiveWeaponIndex + (next ? 1 : -1) * (i+1)) % weaponSlots;
                if(index < 0)
                {
                    index = weaponSlots + index;
                }
                if (weapons[index])
                {
                    ActiveWeaponIndex = index;
                }
            }
        }

        SetActiveWeapon(prevIndex);
    }

    private void SwitchWeaponAtIndex(int index)
    {
        var prevIndex = ActiveWeaponIndex;
        if (weapons[index])
        {
            ActiveWeaponIndex = index;
        }

        SetActiveWeapon(prevIndex);
    }

    public void SetActiveWeapon(int prevIndex)
    {
        if(ActiveWeaponIndex >= 0 && ActiveWeaponIndex != prevIndex && weapons[ActiveWeaponIndex] != null)
        {
            if(prevIndex >= 0)
                weapons[prevIndex].Show(false);
            weapons[ActiveWeaponIndex].Show(true);
        }

    }
}
