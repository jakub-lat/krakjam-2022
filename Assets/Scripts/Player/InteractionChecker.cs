using Cyberultimate.Unity;
using InteractiveObjects;
using UI;
using UnityEngine;

namespace Player
{
    public class InteractionChecker : MonoSingleton<InteractionChecker>
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask layerMask;

        private InteractiveObject currentHit;

        private void Update()
        {
            // Debug.DrawRay(transform.position, transform.forward, Color.red, 2);
            
            if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance, layerMask) &&
                hit.collider.gameObject.CompareTag("Interactable"))
            {
                currentHit = hit.collider.GetComponent<InteractiveObject>();
                InteractionUI.Current?.SetObjectInRange(currentHit);
            }
            else
            {
                currentHit = null;
                InteractionUI.Current?.HideObjectInRange();
            }
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
