using System;
using LetterBattle.Utility;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace UsableItems
{
    public class MeleeWeapon : UsableItem
    {
        [Header("Melee settings")]
        public int damage;
        public float distance;
        public float cooldownTime;
        public float critMultiplier = 1.7f;
        
        private Cooldown cooldown;
        
        [HideInInspector]
        public AudioSource closeFightSource;
        [HideInInspector]
        public AudioClip hitSound;
        
        private void Awake()
        {
            cooldown = new Cooldown(cooldownTime);
            cooldown.AutoUpdate(this);
            
            var rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        public override bool Use()
        {
            if (PauseManager.Current.IsPaused)
            {
                return false;
            }

            if (!cooldown.Push()) return false;

            if (Physics.Raycast(CameraHelper.MainCamera.transform.position, CameraHelper.MainCamera.transform.forward,
                out var hit, distance, HandController.Current.attackLayerMask, QueryTriggerInteraction.Ignore))
            {
                closeFightSource.PlayOneShot(hitSound);
                EnemyDamageUtils.EnemyDamage(hit, damage, damage * critMultiplier, 0);
            }

            return true;
        }
    }
}
