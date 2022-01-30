using Cyberultimate.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GunUI : MonoBehaviour
    {
        [SerializeField] private Text infoText;

        public void SetInfo(string text)
        {
            infoText.text = text;
        }
    }
}
