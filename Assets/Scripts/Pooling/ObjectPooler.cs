using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    public static ObjectPooler instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool p in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for(int i = 0; i < p.size; i++)
            {
                GameObject obj = Instantiate(p.prefab,transform);
                PoolObj po = obj.AddComponent<PoolObj>();

                po.myTag = p.tag;

                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDict.Add(p.tag, objPool);
        }
    }

    public GameObject SpawnPool( string tag, Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (poolDict[tag].Count<=0) 
        { 
            Debug.LogError("POOL QUEUE {" + tag + "} GOT EMPTY");

            return null;
        } 

        obj = poolDict[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;


        //poolDict[tag].Enqueue(obj);

        return obj;
    }
}
