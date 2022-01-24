using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooling Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDic;

    private void Start() {
        poolDic = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDic.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {

        if (!poolDic.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exists.");
            return null;
        }

        GameObject objectToSpawn = poolDic[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDic[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
