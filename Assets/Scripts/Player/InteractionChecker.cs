using System;
using InteractiveObjects;
using UI;
using UnityEngine;

namespace KrakJam2022.Player
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private float maxDistance;

        private InteractiveObject currentHit;

        private void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance) &&
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
