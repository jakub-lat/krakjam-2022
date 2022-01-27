using System;
using System.Collections;
using System.Collections.Generic;
using Cyberultimate.Unity;
using DG.Tweening;
using LetterBattle.Utility;
using UnityEngine;
using UsableItems;

namespace Player
{
    public class HandController : MonoSingleton<HandController>
    {
        [SerializeField] private float pickupTransitionDuration = 0.4f;
        [SerializeField] private LayerMask punchLayerMask;
        [SerializeField] private float punchDistance;
        [SerializeField] private float punchCooldownTime;
        [SerializeField] private float punchDamage;

        private Cooldown punchCooldown;
        
        public UsableItem CurrentItem { get; private set; }

        private void Start()
        {
            punchCooldown = new Cooldown(punchCooldownTime);
            punchCooldown.AutoUpdate(this);
        }

        public void PickUpItem(UsableItem item)
        {
            if (item == null) throw new ArgumentException(nameof(item));
            DropItem();

            CurrentItem = item;
            CurrentItem.OnPickup();


            var rb = CurrentItem.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            CurrentItem.transform.SetParent(transform, true);
            CurrentItem.transform.DOLocalRotate(Vector3.zero, pickupTransitionDuration);
            CurrentItem.transform.DOLocalMove(Vector3.zero, pickupTransitionDuration).SetEase(Ease.InOutQuint);
            CurrentItem.transform.DOScale(Vector3.one, pickupTransitionDuration);
        }

        public void DropItem()
        {
            if (CurrentItem == null) return;

            CurrentItem.transform.SetParent(null, true);
            CurrentItem.OnDrop();
            
            CurrentItem.transform.localScale = Vector3.one;

            var rb = CurrentItem.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // StartCoroutine(PushAfterTime(0.2f, rb, 5f));

            CurrentItem = null;
        }

        // IEnumerator PushAfterTime(float time, Rigidbody rb, float force)
        // {
        //     yield return new WaitForSeconds(time);
        //     Debug.Log(transform.forward);
        //     rb.AddForce(transform.forward.normalized * force, ForceMode.Impulse); 
        // }

        private void PunchAttack()
        {
            if (!punchCooldown.Push()) return;
            
            if (Physics.Raycast(CameraHelper.MainCamera.transform.position, CameraHelper.MainCamera.transform.forward,
                out var hit, punchDistance, punchLayerMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<Enemy>().GotHit(punchDamage);
                    HitmarkManager.Current.GetNormalHit();
                }
                // todo animation
                // todo punching with items?
            }
        }

        public void UseCurrentItem()
        {
            if (CurrentItem == null)
            {
                PunchAttack();
            }
            else
            {
                CurrentItem.Use();
            }
        }
    }
}
