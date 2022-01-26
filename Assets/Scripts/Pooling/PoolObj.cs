using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public string myTag;
    public bool inQueue = true;
    public void Enqueue()
    {
        if (!inQueue)
        {
            inQueue = true;
            ObjectPooler.Current.poolDict[myTag].Enqueue(gameObject);
            transform.parent = ObjectPooler.Current.transform;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inQueue = false;
    }
}
