using Cyberultimate.Unity;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DrugUI : MonoSingleton<DrugUI>
    {
        [SerializeField] private TMP_Text infoText;

        public void SetInfo(string text)
        {
            infoText.text = text;
        }
    }
}
