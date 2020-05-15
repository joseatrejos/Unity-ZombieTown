using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buffs : ScriptableObject
{
    [SerializeField] protected string objectName = "buff";

    [SerializeField] protected bool instaKill = false;
    public bool InstaKill { get => instaKill; set => instaKill = value; }

    [SerializeField] protected float speedBuff = 1;
    public float SpeedBuff { get => speedBuff; set => speedBuff = value; }

    [SerializeField] protected float zombieDamage = 1;
    public float ZombieDamage { get => zombieDamage; set => zombieDamage = value; }

    [SerializeField] protected float bulletDamage = 1;
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }

    public virtual void Use()
    {
        // Aquí pon la animación (si es que hay) de que recuperaste vida
        Debug.Log(objectName + " used!");
    }
}