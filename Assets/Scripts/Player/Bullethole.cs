using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullethole : MonoBehaviour
{
    public float lifetime = 5f;

    private void OnEnable()
    {
        Invoke(nameof(Enqueue), lifetime);
    }

    private void Enqueue()
    {
        if (GetComponent<PoolObj>()) GetComponent<PoolObj>().Enqueue();
        else Destroy(gameObject);
    }
}
