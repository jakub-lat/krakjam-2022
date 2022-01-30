using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private void Start()
    {
        if (PlayerInstance.Current != null)
        {
            if (Vector3.Distance(transform.position, PlayerInstance.Current.transform.position) <=
                ObjectGeneration.Current.minFurnitureRangeFromPlayer)
                Destroy(gameObject);
        }
    }
}
