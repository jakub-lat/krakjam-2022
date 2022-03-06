using System.Collections;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class Landscape : MonoSingleton<Landscape>
    {
        [SerializeField] private int yMultiplier;

        public void SetElevation(int level)
        {
            var newPos = transform.position;
            newPos.y = (level + 2) * yMultiplier;
            transform.position = newPos;
        }
    }
}
