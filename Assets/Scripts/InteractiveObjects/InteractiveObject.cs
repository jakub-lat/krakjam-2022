using UnityEngine;

namespace InteractiveObjects
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [SerializeField] protected bool destroyAfterUse;

        public string interactionName;

        [SerializeField] private AudioSource source;

        protected abstract bool OnInteract();

        public void Interact()
        {
            var res = OnInteract();
            if (res && destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
