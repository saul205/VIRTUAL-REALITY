using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform center;
    public float Height = 2;
    public float Radius = 5;
    public LayerMask PickMask;

    public AmmoManager ammoManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var items = Physics.OverlapSphere(center.position, Radius, PickMask);
        foreach(var item in items)
        {
            var pickup = item.gameObject.GetComponentInParent<IPickup>();
            if(pickup != null && pickup.CanPickup)
            {
                var ammo = item.gameObject.GetComponentInParent<Shootable>();
                if(ammo != null)
                {
                    if(ammoManager.AddAmmo(ammo.prefab, pickup.value))
                    {
                        Destroy(item.gameObject.transform.parent.gameObject);
                    }
                }
                else
                {
                    Destroy(item.gameObject.transform.parent.gameObject);
                }
            }  
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center.position, Radius);
    }
}