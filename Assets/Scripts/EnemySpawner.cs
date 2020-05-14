using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }
    void FixedUpdate()
    {
        objectPooler.SpawnFromPool("Enemy", this.transform.position);
    }
}
