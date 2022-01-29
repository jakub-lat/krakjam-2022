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
        [SerializeField] private UsableItem fist;
        public LayerMask attackLayerMask;

        public UsableItem CurrentItem { get; private set; }

        private Vector3 originalScale;
        private int originalLayer;

        [SerializeField]
        private AudioSource closeFightSource;

        [SerializeField]
        private AudioClip fistPunch;

        [SerializeField]
        private AudioClip itemPunch;
        
        public void PickUpItem(UsableItem item)
        {
            if (item == null) throw new ArgumentException(nameof(item));
            DropItem();

            CurrentItem = item;
            CurrentItem.OnPickup();

            var rb = CurrentItem.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            
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

            CurrentItem = null;
        }


        public void UseCurrentItem()
        {
            if (CurrentItem == null)
            {
                closeFightSource.PlayOneShot(fistPunch);
                fist.Use();
                PlayerAnim.Current.Punch();
            }
            else
            {
                closeFightSource.PlayOneShot(itemPunch);
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
