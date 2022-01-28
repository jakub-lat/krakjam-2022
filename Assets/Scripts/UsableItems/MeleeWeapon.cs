using System;
using LetterBattle.Utility;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace UsableItems
{
    public class MeleeWeapon : UsableItem
    {
        public int damage;
        public float distance;
        public float cooldownTime;
        
        private Cooldown cooldown;

        private void Awake()
        {
            cooldown = new Cooldown(cooldownTime);
            cooldown.AutoUpdate(this);
            
            var rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        public override void Use()
        {
            if (!cooldown.Push()) return;
            
            if (Physics.Raycast(CameraHelper.MainCamera.transform.position, CameraHelper.MainCamera.transform.forward,
                out var hit, distance, HandController.Current.attackLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.transform.GetComponentInParent<Enemy>();
                    enemy.GotHit(damage);
                    HitmarkManager.Current.GetNormalHit();
                    PopupManager.Current.SpawnStandardDamage(enemy, damage);
                }
                // todo animation
            }

        }
    }
}
