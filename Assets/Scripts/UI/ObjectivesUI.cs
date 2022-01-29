using System;
using Cyberultimate.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ObjectivesUI : MonoSingleton<ObjectivesUI>
    {
        [SerializeField] private Text objectiveText;
        [SerializeField] private Text objectiveDescription;

        private RectTransform rt;
        private CanvasGroup canvasGroup;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.DOFade(0, 0);
        }

        public void SetObjective(string title, string description)
        {
            objectiveText.text = title;
            objectiveDescription.text = description;

            rt.localScale = new Vector3(4, 4, 4);
            canvasGroup.DOFade(0, 0);

            canvasGroup.DOFade(1, 0.6f).SetEase(Ease.OutCubic);
            rt.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.InCirc);

            canvasGroup.DOFade(0, 1.5f).SetEase(Ease.InOutCubic).SetDelay(7f);

            /*
            rt.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f).SetDelay(2f);
            // rt.DOSizeDelta(new Vector2(600, rt.sizeDelta.y), 1f).SetDelay(2f);
            rt.DOAnchorPosX(700f, 0.5f).SetDelay(2f);
        */
        }
    }
}
