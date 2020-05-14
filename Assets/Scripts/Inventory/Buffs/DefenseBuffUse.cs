using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuffUse : MonoBehaviour
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
