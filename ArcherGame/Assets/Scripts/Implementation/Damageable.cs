using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    #region Damageable

    public int hp = 0;
    public int MaxHp = 100;
    public virtual bool CanTakeDmg { get; set; } =  true;

    public void Start()
    {
        Hp = MaxHp;
    }
    public int Hp { get { return hp; } set { hp = value; } }

    public void Hit(int dmg)
    {
        if(CanTakeDmg) { 
            Hp -= dmg;
            if (Hp <= 0)
                Die();
            else
            {
                AfterHit();
            }
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void AfterHit()
    {

    }

    #endregion
}
