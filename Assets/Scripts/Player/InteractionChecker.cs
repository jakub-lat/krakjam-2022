using InteractiveObjects;
using UI;
using UnityEngine;

namespace Player
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask layerMask;

        private InteractiveObject currentHit;

        private void Update()
        {
            if (!InteractionUI.Current) return;

            if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance, layerMask) &&
                hit.collider.gameObject.CompareTag("Interactable"))
            {
                currentHit = hit.collider.GetComponent<InteractiveObject>();
                InteractionUI.Current.SetObjectInRange(currentHit);
            }
            else
            {
                currentHit = null;
                InteractionUI.Current.HideObjectInRange();
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
