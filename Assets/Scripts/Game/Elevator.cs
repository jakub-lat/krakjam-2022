using System;
using UnityEngine;

namespace Game
{
    public class Elevator : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                LevelManager.Current.NextLevel();
            }
        }
    }
}
