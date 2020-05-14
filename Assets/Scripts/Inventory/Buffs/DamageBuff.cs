using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buffs", menuName = "Items/DamageBuff", order = 1)]

public class DamageBuff : Buffs
{
    public override void Use()
    {
        base.Use();
        bulletDamage = 1.5f;
    }
}
