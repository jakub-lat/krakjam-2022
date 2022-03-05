using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorRemover : MonoBehaviour
{
    public float removeDist = 2f;
    public LayerMask removeMask;

    public void Remove()
    {
        // Debug.Log("BRR");
        foreach(Transform o in GenerateRoom.Current.GetComponentsInChildren<Transform>())
        {
            if (Vector3.Distance(o.position,transform.position)<=removeDist && removeMask == (removeMask | (1 << o.gameObject.layer)))
            {
                o.gameObject.SetActive(false);
            }
        }
    }
}
