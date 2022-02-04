using System;
using System.Linq;
using InteractiveObjects;
using Player;
using UnityEngine;

namespace UsableItems
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class UsableItem : InteractiveObject
    {
        [Header("Usable item")]
        public Vector3 rotationOffset;
        public Vector3 positionOffset;

        public float throwDamage = 10f;
        public float throwHeadshotDamage = 15f;
        
        public abstract void Use();

        public virtual void OnPickup()
        {
        }

        public virtual void OnDrop()
        {
        }

        private void OnCollisionEnter(Collision other)
        {
            EnemyDamageUtils.EnemyDamage(other, throwDamage, throwHeadshotDamage, 0);
        }

        protected override bool OnInteract()
        {
            HandController.Current.PickUpItem(this);
            return true;
        }
    }
}
