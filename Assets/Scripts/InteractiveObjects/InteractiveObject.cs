using UnityEngine;

namespace InteractiveObjects
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [SerializeField] protected bool destroyAfterUse;
        
        protected abstract void OnInteract();

        public void Interact()
        {
            OnInteract();
            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
