using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buffs : ScriptableObject
{
    protected bool instaKill = false;
    protected float speedBuff = 1;
    protected float zombieDamage = 1;
    protected float bulletDamage = 1;

    public virtual void Use()
    {
        // Aquí pon la animación (si es que hay) de que recuperaste vida
    }
}