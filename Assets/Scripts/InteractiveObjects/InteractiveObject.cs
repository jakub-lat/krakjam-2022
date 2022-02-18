using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
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
        
        private List<MonoBehaviour> outlines;
        
        private void Start()
        {
            // outlines = gameObject.GetComponentsInChildren<Renderer>().Select(x => x.AddComponent<Outline>()).ToList();
            // foreach (var outline in outlines)
            // {
            //     outline.enabled = false;
            // }
            // outline.OutlineColor = Color.yellow;
            // outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
            // outline.OutlineWidth = 10f;
            // outline.enabled = false;
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

        public virtual void OnHover()
        {
            // foreach (var outline in outlines)
            // {
            //     outline.enabled = true;
            // }
            // CameraHelper.MainCamera.GetComponent<OutlineEffect>
        }

        public virtual void OnHoverEnd()
        {
            // foreach (var outline in outlines)
            // {
            //     outline.enabled = false;
            // }
        }
    }
}
