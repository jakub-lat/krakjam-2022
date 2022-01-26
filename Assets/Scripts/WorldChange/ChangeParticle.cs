using System;
using System.Collections.Generic;
using DG.Tweening;
using KrakJam2022.Player;
using Player;
using UnityEngine;
using Cyberultimate.Unity;

namespace WorldChange
{
    public class ChangeParticle : WorldChangeLogic
    {

        public static ChangeParticle instance;
        
        [SerializeField] private WorldTypeDict<List< ParticleSystem.MinMaxGradient>> colors;

        [HideInInspector]
        public List<ParticleSystem.MinMaxGradient> curr;

        private void Awake()
        {
            instance = this;
            curr = colors[WorldTypeController.WorldType.Normal];
        }

        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            curr = colors[type];
        }
        
    }
}
