using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public GameObject target;
    public int hp = 100;
    public int Hp { get
        {
            return hp;
        }
        set 
        {
            hp = value;
        } 
    }
    public void Hit(int dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
            Die();
    }

    public void Die()
    {
        var arrows = gameObject.GetComponentsInChildren<Shootable>();
        foreach(Shootable sh in arrows)
        {
            sh.transform.parent = null;
        }
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float speed = 5;
    void Update()
    {
        transform.LookAt(target.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
