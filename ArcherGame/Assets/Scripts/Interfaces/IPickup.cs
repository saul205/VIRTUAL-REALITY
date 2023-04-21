using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    public bool CanPickup { get; set; }

    public int value { get; set; }
   
}
