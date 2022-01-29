using System;
using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct GenObject
{
    public ObjectGeneration.ObjectType type;
    public int count;
    public GameObject prefab;
}

public class ObjectGeneration : MonoSingleton<ObjectGeneration>
{
    public enum ObjectType
    {
        Ammo,
        Pills,
        Gun
    }

    [HideInInspector] public Dictionary<ObjectType, List<GameObject>> dict;

    public List<GenObject> objects;
    public float minRangeFromPlayer = 10f;
    public float minFurnitureRangeFromPlayer = 5f;

    private new void Awake()
    {
        base.Awake();
        dict = new Dictionary<ObjectType, List<GameObject>>();

        ClearObjects();
    }

    public void ClearObjects()
    {
        foreach (ObjectType type in Enum.GetValues(typeof(ObjectType)))
        {
            dict[type] = new List<GameObject>();
        }    
    }

    public void GenerateObjects()
    {
        foreach (GenObject go in objects) {
            for (int i = 0; i < go.count; i++)
            {
                
                int places = dict[go.type].Count;
                if(places<=0) { Debug.LogWarning("Not enough places to spawn " + go.type); break; }

                int index = Random.Range(0, places);

                // spawn in random pos
                Transform t = dict[go.type][index].transform;
                dict[go.type].RemoveAt(index);

                var spawned = Instantiate(go.prefab, t.position, t.rotation, t);
                var tr = spawned.transform;
                if (go.type == ObjectType.Gun)
                {
                    var newscale = Vector3.one;
                    tr.localScale = new Vector3 (newscale.x/tr.lossyScale.x, newscale.y/tr.lossyScale.y, newscale.z/tr.lossyScale.z);
                }
            }
        }
    }
}
