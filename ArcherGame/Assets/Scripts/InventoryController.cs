using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public List<Item> inventory;
    public int MaxInv = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddToInventory(Item obj)
    {
        if(inventory.Count < MaxInv)
        {
            inventory.Add(obj);
            return true;
        }

        return false;
    }

    public Item GetKey()
    {
        if(inventory.Count > 0)
        {
            var a = inventory[0];
            inventory.RemoveAt(0);
            return a;
        }

        return null;
    }

    public void RemoveFromInventory(Item obj)
    {
        inventory.Remove(obj);
    }

}
