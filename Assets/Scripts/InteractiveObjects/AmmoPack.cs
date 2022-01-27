using System;
using KrakJam2022.Player;
using Player;
using UnityEngine;
using UsableItems;

namespace InteractiveObjects
{
    public class AmmoPack : InteractiveObject
    {
        [SerializeField] private int amount;

        protected override bool OnInteract()
        {
            if (Gun.Current == null) return false;
            
            Gun.Current.AddAmmo(amount);
            return true;
        }
    }
}
