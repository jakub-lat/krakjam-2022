using Cyberultimate.Unity;
using InteractiveObjects;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class InteractionUI : MonoSingleton<InteractionUI>
    {
        [SerializeField] private GameObject interactionAvailableInfo;
        [SerializeField] private Text objectNameText;

        public void SetObjectInRange(InteractiveObject obj)
        {
            if (obj == null) return;
            interactionAvailableInfo.SetActive(true);
            objectNameText.text = $"{obj.interactionName} - E";
        }

        public void HideObjectInRange()
        {
            interactionAvailableInfo.SetActive(false);
        }
    }
}
