using UnityEngine;

namespace InteractiveObjects
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [SerializeField] protected bool destroyAfterUse;

        public string interactionName;

        [SerializeField] private AudioClip interactionSound;
        [SerializeField] private AudioSource source;

        protected abstract bool OnInteract();

        public void Interact()
        {
            var res = OnInteract();
            source?.PlayOneShot(interactionSound);
            if (res && destroyAfterUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
