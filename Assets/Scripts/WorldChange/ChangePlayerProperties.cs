using System;
using Cyberultimate.Unity;
using KrakJam2022.Player;
using StarterAssets;
using UnityEngine;
using WorldChange;

namespace Player
{
    [Serializable]
    public class PlayerPropertiesData
    {
        public float moveSpeed;
    }
    
    public class ChangePlayerProperties : WorldChangeLogic
    {
        [SerializeField]
        private WorldTypeDict<PlayerPropertiesData> data;

        private FirstPersonController fpsc;

        private void Start()
        {
            fpsc = GetComponent<FirstPersonController>();
        }

        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            var properties = data[type];
            fpsc.MoveSpeed = properties.moveSpeed;
        }
    }
}
