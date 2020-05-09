using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion", order = 1)]

public class Potion : Consumable
{
    [SerializeField]
    int healthPoints = 0;
    
    [SerializeField]
    int manaPoints = 0;

    public int RestoreHealth { get => healthPoints; }
    public int RestoreMana { get => manaPoints; }

    public override void Drink()
    {
        base.Drink();
        Debug.Log(RestoreHealth + " health points recovered");
    }
}
