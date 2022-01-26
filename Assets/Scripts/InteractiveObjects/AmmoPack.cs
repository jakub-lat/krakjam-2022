using System;
using KrakJam2022.Player;
using UnityEngine;

namespace InteractiveObjects
{
    public class AmmoPack : InteractiveObject
    {
        [SerializeField] private int amount;

        protected override void OnInteract()
        {
            GunController.Current.AddAmmo(amount);
        }
    }
}
