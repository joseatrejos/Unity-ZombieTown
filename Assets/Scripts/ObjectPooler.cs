using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        // Aqui se dice el tag que tendra el objeto que spawnea
        public string tag;

        // Aqui se arrastra el prefab
        public GameObject prefab;

        // Aqui con cuantos se va a empezar
        public int size;
    }

    private bool spawned = false;

    public Pool pool;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }



    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool tag" + tag + "no existe");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        if (!spawned)
        {
            objectToSpawn.transform.position += new Vector3(Random.Range(-8.0f, 8.0f), 0, Random.Range(-8.0f, 8.0f));
            objectToSpawn.GetComponent<Collider>().isTrigger = false;
            spawned = true;
        }

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public GameObject AddEnemiesToPool(string tag)
    {
        pools[0].size++;
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size-1; i++)
                {
                    Debug.Log(pool.size);
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(true);
                    objectPool.Enqueue(obj);
                }
                poolDictionary[tag].Enqueue(Instantiate(pool.prefab));
        }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            if (!spawned)
            {
                objectToSpawn.GetComponent<Collider>().isTrigger = false;
                spawned = true;
            }

            return objectToSpawn;
    }

}
