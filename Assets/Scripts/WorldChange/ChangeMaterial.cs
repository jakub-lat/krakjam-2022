using System;
using System.Collections.Generic;
using KrakJam2022.Player;
using UnityEngine;

namespace WorldChange
{
    public class ChangeMaterial : WorldChangeLogic
    {
        [SerializeField] private WorldTypeDict<Material> materials;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            meshRenderer.material = materials[type];
        }
    }
}
