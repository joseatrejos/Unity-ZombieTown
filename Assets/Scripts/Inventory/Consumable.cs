using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public virtual void Drink()
    {
        Debug.Log(name + " Consumed");
        // Aquí pon la animación (si es que hay) de que recuperaste vida
    }
}
