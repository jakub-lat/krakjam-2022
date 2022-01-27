using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialPlace : MonoBehaviour
{
    public ObjectGeneration.ObjectType type;

    private void Awake()
    {
        ObjectGeneration.Current.dict[type].Add(gameObject);
        // Debug.Log(ObjectGeneration.Current.dict[type].Count);
    }
}
