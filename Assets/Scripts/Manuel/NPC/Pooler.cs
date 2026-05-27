using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] GameObject NPC;
    [SerializeField] List<GameObject> pool_NPC = new List<GameObject>();
    int instancedCount, PooledCount;
    public GameObject getNPC(Vector3 position)
    {
        return SpawnNPCFromPool(NPC, pool_NPC, position);
    }
    private GameObject SpawnNPCFromPool(GameObject prefab, List<GameObject> pool, Vector3 position)
    {
        for(int i = 0; i < pool.Count; i++)
        {
            if(pool[i].activeInHierarchy == false)
            {
                pool[i].transform.position = position;
                pool[i].SetActive(true);
                PooledCount++;
                return pool[i];
            }
        }
        GameObject instancedObj = Instantiate(prefab, position, Quaternion.identity);
        pool.Add(instancedObj);
        instancedCount++;
        return instancedObj;
    }
}
