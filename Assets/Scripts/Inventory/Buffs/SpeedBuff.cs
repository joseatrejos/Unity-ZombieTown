using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buffs", menuName = "Items/SpeedBuff", order = 1)]

public class SpeedBuff : Buffs
{
    public override void Use()
    {
        base.Use();
        speedBuff = 1.5f;
    }
}
