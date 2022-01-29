using System;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class GameMusic : MonoSingleton<GameMusic>
    {
        public AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
        }
    }
}
