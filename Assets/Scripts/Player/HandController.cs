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
    [RequireComponent(typeof(AudioSource))]
    public class HandController : MonoSingleton<HandController>
    {
        [SerializeField] private float pickupTransitionDuration = 0.4f;
        [SerializeField] private float throwItemForce = 8f;
        [SerializeField] private MeleeWeapon fist;
        public LayerMask attackLayerMask;

        [SerializeField] private UsableItem startingItem;
        [SerializeField] private AudioClip meleeSound;

        public UsableItem CurrentItem { get; private set; }

        private Vector3 originalScale;
        private int originalLayer;

        private void Start()
        {
            fist.closeFightSource = GetComponent<AudioSource>();
            fist.hitSound = meleeSound;

            
            if (startingItem != null)
            {
                var go = Instantiate(startingItem);
                go.transform.localScale = Vector3.one;
                PickUpItem(go);
            }
        }

        public void PickUpItem(UsableItem item)
        {
            if (item == null) throw new ArgumentException(nameof(item));
            DropItem();

            CurrentItem = item;
            CurrentItem.OnPickup();
            
            var rb = CurrentItem.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            
            foreach (var col in CurrentItem.GetComponents<Collider>())
            {
                col.enabled = false;
            }

            originalScale = CurrentItem.transform.lossyScale;
            originalLayer = CurrentItem.gameObject.layer;

            SetLayerRecursively(CurrentItem.gameObject, gameObject.layer);
            CurrentItem.transform.SetParent(transform, true);
            CurrentItem.transform.DOLocalRotate(CurrentItem.rotationOffset, pickupTransitionDuration);
            CurrentItem.transform.DOLocalMove(CurrentItem.positionOffset, pickupTransitionDuration).SetEase(Ease.InOutQuint);
            CurrentItem.transform.DOScale(CurrentItem.transform.localScale * transform.localScale.x, pickupTransitionDuration);
            CurrentItem.tag = "Untagged";
        }

        public void DropItem()
        {
            if (CurrentItem == null) return;

            // var pos = CurrentItem.transform.position;
            // CurrentItem.transform.SetParent(null, false);
            // CurrentItem.transform.position = pos;

            SetLayerRecursively(CurrentItem.gameObject, originalLayer);
            CurrentItem.transform.SetParent(null, true);
            CurrentItem.transform.localScale /= transform.localScale.x;

            CurrentItem.OnDrop();
            CurrentItem.tag = "Interactable";
            // CurrentItem.transform.localScale = originalScale;
            
            // CurrentItem.transform.localScale = Vector3.one;
            
            foreach (var col in CurrentItem.GetComponents<Collider>())
            {
                col.enabled = true;
            }

            var rb = CurrentItem.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            
            rb.AddForce(CameraHelper.MainCamera.transform.forward * throwItemForce, ForceMode.VelocityChange);
            // StartCoroutine(PushAfterTime(0.2f, rb, 15f));

            CurrentItem = null;
        }

        /*IEnumerator PushAfterTime(float time, Rigidbody rb, float force)
        {
            yield return new WaitForSeconds(time);
        }*/


        public void UseCurrentItem()
        {
            if (CurrentItem is MeleeWeapon m)
            {
                m.closeFightSource = GetComponent<AudioSource>();
                m.hitSound = meleeSound;
            }

            
            if (CurrentItem == null)
            {
                fist.Use();
                PlayerAnim.Current.Punch();
            }
            else
            {
                if (CurrentItem is MeleeWeapon)
                {
                    PlayerAnim.Current.ItemHit();
                }
                
                CurrentItem.Use();
            }
        }
        
        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }
       
            obj.layer = newLayer;
       
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
