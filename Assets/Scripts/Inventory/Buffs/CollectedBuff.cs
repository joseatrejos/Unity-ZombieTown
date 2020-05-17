using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedBuff : MonoBehaviour
{
    [SerializeField] Buffs buffs;

    public Buffs Buffs { get => buffs; }

    public void ApplySpeedBuff(Player playerToBuff)
    {
        buffs.Use();
        playerToBuff.moveSpeed *= buffs.SpeedBuff;
    }

    public void ApplyDamageBuff()
    {
        buffs.Use();
        GameManager.instance.bulletDamage *= buffs.BulletDamage;
    }

    public void ApplyDefenseBuff()
    {
        buffs.Use();
        GameManager.instance.Invencible.SetActive(true);
        GameManager.instance.zombieDamage *= buffs.ZombieDamage;
    }

    public void ApplyInstaKillBuff()
    {
        buffs.Use();
        GameManager.instance.instakillBuff = buffs.InstaKill;
    }
}
