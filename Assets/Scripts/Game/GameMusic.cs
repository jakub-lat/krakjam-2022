using System;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class GameMusic : MonoSingleton<GameMusic>
    {
        [HideInInspector]
        public AudioSource audioSource;

        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            base.Awake();
        }
    }
}
