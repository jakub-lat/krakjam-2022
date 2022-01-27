using Cyberultimate.Unity;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class DrugUI : MonoSingleton<DrugUI>
    {
        [SerializeField] private GameObject drugPrefab = null;
        [SerializeField] private Transform drugParent = null;  // don't do drugs, kids.

        public void SetInfo(int howMany)
        {
            drugParent.KillAllChildren(); // i warned ya
            for (int i = 0; i < howMany; i++)
            {
                Instantiate(drugPrefab, drugParent);
            }
        }
    }
}
