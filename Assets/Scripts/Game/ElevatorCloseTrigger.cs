using System;
using UnityEngine;

namespace Game
{
    public class ElevatorCloseTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GetComponentInParent<Elevator>().Use();
            }
        }
    }
}
