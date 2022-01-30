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
        public float critMultiplier = 1.7f;
        
        private Cooldown cooldown;

        [SerializeField]
        private AudioSource closeFightSource;

        [SerializeField]
        private AudioClip fistPunch;

        [SerializeField]
        private AudioClip itemPunch;

        private void Awake()
        {
            cooldown = new Cooldown(cooldownTime);
            cooldown.AutoUpdate(this);
            
            var rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        public override void Use()
        {
            if (PauseManager.Current.IsPaused)
            {
                return;
            }

            if (!cooldown.Push()) return;
            
            if (Physics.Raycast(CameraHelper.MainCamera.transform.position, CameraHelper.MainCamera.transform.forward,
                out var hit, distance, HandController.Current.attackLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    if (hit.transform.GetComponentInParent<Enemy>()) //its a normal enemy
                    {
                        Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                        enemy.GotHit(damage);
                    }
                    else if (hit.transform.GetComponentInParent<Boss>()) //its a boss
                    {
                        Boss boss = hit.transform.GetComponentInParent<Boss>();
                        boss.GotHit(damage);
                    }
                    else
                    {
                        Debug.LogError("Enemy doesnt have a enemy or boss component");
                    }

                    HitmarkManager.Current.GetNormalHit();
                    PopupManager.Current.SpawnStandardDamage(hit.transform.parent, damage);
                }
                else if (hit.collider.gameObject.CompareTag("EnemyHead"))
                {
                    if (hit.transform.GetComponentInParent<Enemy>()) //its a normal enemy
                    {
                        Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                        enemy.GotHit(damage*critMultiplier);
                    }
                    else if (hit.transform.GetComponentInParent<Boss>()) //its a boss
                    {
                        Boss boss = hit.transform.GetComponentInParent<Boss>();
                        boss.GotHit(damage * critMultiplier);
                    }
                    else
                    {
                        Debug.LogError("Enemy doesnt have a enemy or boss component");
                    }

                    HitmarkManager.Current.GetHeadshotHit();
                    closeFightSource.PlayOneShot(fistPunch);
                    PopupManager.Current.SpawnHeadshotDamage(hit.transform.parent, (int)(damage * critMultiplier));
                }

                // todo animation
            }

        }
    }
}
