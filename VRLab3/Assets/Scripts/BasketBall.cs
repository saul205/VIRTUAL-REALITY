using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    public bool canPickup = true;
    private float timeShoot = 0;
    private float pickDelay = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeShoot + pickDelay){
            canPickup = true;
        }
    }

    public void Shoot()
    {
        transform.SetParent(null);
        canPickup = false;
        timeShoot = Time.time;
    }

}
