using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
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
        int x_axis = Random.Range(-1, 2);
        int y_axis = Random.Range(-1, 2);

        transform.position = transform.position + new Vector3(x_axis, 0, y_axis) * speed * Time.deltaTime;
    }
}
