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
        GameManager.instance.zombieDamage *= buffs.ZombieDamage;
    }

    public void ApplyInstaKillBuff()
    {
        buffs.Use();
        GameManager.instance.instakillBuff = buffs.InstaKill;
    }

    // Reset Buff stats after 12 seconds
    public IEnumerator ResetBuffs(string buff)
    {
        Debug.Log("Se quiere resetear");
        yield return new WaitForSeconds(12);

        switch(buff)
        {
            case "damage":
                Debug.Log(buff + " reset");
                GameManager.instance.bulletDamage /= buffs.BulletDamage;
            break;

            case "defense":
                Debug.Log(buff + " reset");
                GameManager.instance.zombieDamage /= buffs.ZombieDamage;  
            break;

            case "instakill":
                Debug.Log(buff + " reset");
                GameManager.instance.instakillBuff = false;
            break;
        }
    }
}
