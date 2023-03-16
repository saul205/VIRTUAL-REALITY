using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    #region Damageable

    public int hp = 0;
    public int Hp { get { return hp; } set { hp = value; } }

    public void Hit(int dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    #endregion
}
