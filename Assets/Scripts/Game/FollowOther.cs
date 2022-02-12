using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FollowOther : MonoBehaviour
    {
        [SerializeField] private Transform other;

        private void Update()
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
        }
    }
}
