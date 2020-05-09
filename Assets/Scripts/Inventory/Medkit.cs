using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Medkit", menuName = "Items/Medkit", order = 2)]

public class Medkit : Consumable
{
    [SerializeField]
    int healthPoints = 70;

    public int HealthPoints { get => healthPoints; }

    public override void Drink()
    {
        base.Drink();
    }
}