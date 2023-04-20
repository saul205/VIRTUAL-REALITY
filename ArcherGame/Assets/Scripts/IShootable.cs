using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IShootable
{
    public float Speed { get; set; }
    public float MaxDistance { get; set; }

    public int damage { get; set; }
    public void Fly();
    public void Shoot(WeaponController controller);
}
