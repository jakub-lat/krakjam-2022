using Cyberultimate.Unity;
using InteractiveObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InteractionUI : MonoSingleton<InteractionUI>
    {
        [SerializeField] private GameObject interactionAvailableInfo;
        [SerializeField] private TMP_Text objectNameText;

        public void SetObjectInRange(InteractiveObject obj)
        {
            interactionAvailableInfo.SetActive(true);
            objectNameText.text = obj.interactionName + " (E)";
        }

        public void HideObjectInRange()
        {
            interactionAvailableInfo.SetActive(false);
        }
    }
}
