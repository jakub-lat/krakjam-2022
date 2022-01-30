using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform target;
    public Vector3 targetEuler;

    public void DoIt()
    {
        target.DOLocalRotate(targetEuler, 1f);
    }

    bool did = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!did && other.CompareTag("Player"))
        {
            did = true;
            DoIt();
        }
    }
}
