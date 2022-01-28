using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialPlace : MonoBehaviour
{
    public ObjectGeneration.ObjectType type;

    private void Awake()
    {
        if (Vector3.Distance(transform.position, PlayerInstance.Current.transform.position) <= ObjectGeneration.Current.minRangeFromPlayer) Destroy(gameObject);
        ObjectGeneration.Current.dict[type].Add(gameObject);
    }
}
