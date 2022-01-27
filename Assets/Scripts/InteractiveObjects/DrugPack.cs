using System;
using KrakJam2022.Player;
using Player;
using UnityEngine;

namespace InteractiveObjects
{
    public class DrugPack : InteractiveObject
    {
        [SerializeField] private int amount;

        protected override bool OnInteract()
        {
            DrugController.Current.AddDrugs(amount);
            return true;
        }
    }
}
