using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Pills
    }

    [HideInInspector] public Dictionary<ObjectType, List<GameObject>> dict;

    public List<GenObject> objects;

    private new void Awake()
    {
        base.Awake();
        dict = new Dictionary<ObjectType, List<GameObject>>();
        dict[ObjectType.Ammo] = new List<GameObject>();
        dict[ObjectType.Pills] = new List<GameObject>();
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

                Instantiate(go.prefab, t.position, t.rotation, t);
            }
        }
    }
}
