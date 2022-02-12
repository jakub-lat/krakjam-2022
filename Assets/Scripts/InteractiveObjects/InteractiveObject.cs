using System;
using QuickOutline;
using UnityEngine;

namespace InteractiveObjects
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [Header("Interactive object")]
        [SerializeField] protected bool destroyAfterUse;

        public string interactionName;

        [SerializeField] private AudioSource source;

        private void Start()
        {
            // if (gameObject.name.ToLower().Contains("gun"))
            // {
            //     var outline = gameObject.AddComponent<Outline>();
            //     outline.OutlineColor = Color.red;
            //     outline.OutlineWidth = 5f;
            // }
            // outline.OutlineMode = Outline.Mode.OutlineVisible;
        }

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
