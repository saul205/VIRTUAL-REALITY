using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public List<Shootable> ammoTypes;
    public List<int> ammoMax;
    public Dictionary<Shootable, int> ammoCounter = new Dictionary<Shootable, int>();
    public Dictionary<Shootable, int> ammoCounterMax = new Dictionary<Shootable, int>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < ammoTypes.Count; i++)
        {
            ammoCounter[ammoTypes[i]] = ammoMax[i];
            ammoCounterMax[ammoTypes[i]] = ammoMax[i];
        }
    }

    public int TakeAmmo(Shootable type, int amount)
    {
        var availableAmmo = ammoCounter[type];
        if (availableAmmo >= amount)
        {
            ammoCounter[type] -= amount;
            return amount;
        }
        else
        {
            ammoCounter[type] = 0;
            return availableAmmo;
        }
    }

    public bool AddAmmo(Shootable type, int amount)
    {
        
        if (ammoCounter[type] < ammoCounterMax[type])
        {
            ammoCounter[type] += amount;
            if(ammoCounter[type] > ammoCounterMax[type])
                ammoCounter[type] = ammoCounterMax[type];
            return true;
        }

        return false;
    }

    public string GetAllAmmoText()
    {
        string ammo = "";
        foreach(var key in ammoCounter)
        {
            ammo += key.Value + "\n";
        }
        return ammo;
    }
}
