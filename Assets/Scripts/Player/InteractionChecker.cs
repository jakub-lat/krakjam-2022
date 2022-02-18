using System;
using Cyberultimate.Unity;
using InteractiveObjects;
using UI;
using UnityEngine;
using UnityFx.Outline;

namespace Player
{
    public class InteractionChecker : MonoSingleton<InteractionChecker>
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask layerMask;

        private InteractiveObject currentHit;

        private OutlineEffect outlineEffect;
        private OutlineLayer outlineLayer;
        
        private void Start()
        {
            outlineEffect = CameraHelper.MainCamera.GetComponent<OutlineEffect>();
            outlineLayer = outlineEffect.OutlineLayers[0];
            HitEnd();
            // outlineEffect.AddGameObject();
        }

        private void Update()
        {
            // Debug.DrawRay(transform.position, transform.forward, Color.red, 2);

            if (!Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance, layerMask, QueryTriggerInteraction.Collide) ||
                !hit.collider.gameObject.CompareTag("Interactable"))
            {
                HitEnd();
                return;
            }
            
            var newHit = hit.collider.GetComponent<InteractiveObject>();
            if (newHit == currentHit) return;
            
            HitEnd();
            currentHit = newHit;
            outlineLayer.Add(currentHit.gameObject);
            currentHit.OnHover();
            InteractionUI.Current?.SetObjectInRange(currentHit);
        }

        private void HitEnd()
        {
            if (currentHit == null) return;
            
            outlineLayer.Remove(currentHit.gameObject);
            currentHit.OnHoverEnd();
            currentHit = null;
            InteractionUI.Current?.HideObjectInRange();
        }

        public void OnInteract()
        {
            if (currentHit != null)
            {
                currentHit.Interact();
            }
        }
    }
}
