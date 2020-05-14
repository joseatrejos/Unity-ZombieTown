﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuffUse : MonoBehaviour
{
    [SerializeField]
    Buffs buffs;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            buffs.Use();
            Destroy(this.gameObject);
        }
    }
}