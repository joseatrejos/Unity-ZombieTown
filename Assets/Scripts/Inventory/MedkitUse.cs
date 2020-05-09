using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitUse : MonoBehaviour
{
    [SerializeField]
    Medkit medkit;

    public int Use()
    {
        return medkit.HealthPoints;
    }
}