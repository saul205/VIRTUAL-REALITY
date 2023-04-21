using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float rof = 120;
    private float delay = 0;
    // Start is called before the first frame update
    private bool shoot = false;
    void Start()
    {
        
    }

    private void OnShoot()
    {
        shoot = !shoot;
        delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(shoot && bulletPrefab != null)
        {
            if(delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else {         
                var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);

                delay = 60f / rof;
            } 
        }
    }
}
