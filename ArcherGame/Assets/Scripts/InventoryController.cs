using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    List<Object> inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(Object obj)
    {
        inventory.Add(obj);
    }

    public void RemoveFromInventory(Object obj)
    {
        inventory.Remove(obj);
    }

}
