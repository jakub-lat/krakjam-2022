using InteractiveObjects;
using UnityEngine;

namespace KrakJam2022.Player
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        
        public void OnInteract()
        {
            // todo feedback kiedy jest się w zasięgu obiektu (np podświetlenie go)
            if (Physics.Raycast(transform.position, transform.forward, out var hit, maxDistance))
            {
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    hit.collider.GetComponent<InteractiveObject>().Interact();
                }
            }
        }
    }
}
