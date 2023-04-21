using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour, IBodyPart, IDamageable
{
    public GameObject body;
    public int multiplier = 1;
    public int Multiplier { get
        {
            return multiplier;
        }
        set
        {
            multiplier = value;
        }
    }

    public int Hp { get; set; }

    public void Die()
    {
        return;
    }

    public void Hit(int dmg)
    {
        body.GetComponent<IDamageable>().Hit(dmg * multiplier);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
