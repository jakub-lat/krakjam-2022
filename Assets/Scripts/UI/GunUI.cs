using Cyberultimate.Unity;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GunUI : MonoBehaviour
    {
        [SerializeField] private Text infoText;

        [SerializeField]
        private CanvasGroup group;
        public CanvasGroup Group => group;

        public void SetInfo(string text)
        {
            infoText.text = text;
        }
    }
}
