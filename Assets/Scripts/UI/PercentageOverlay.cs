using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum OverlayType
    {
        Health
    }

    public class PercentageOverlay : MonoBehaviour
    {
        public OverlayType overlayType;

        [SerializeField] private float transitionDuration;

        private float amount;

        private Image image;
        private RectTransform rectTransform;

        private static Dictionary<OverlayType, PercentageOverlay> _instances = new();

        private void Awake()
        {
            _instances[overlayType] = this;
        }

        private void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            UpdateAmount(0);
        }

        public static PercentageOverlay Get(OverlayType type) => _instances[type];

        public void UpdateAmount(float newAmount)
        {
            // Debug.Log("overlay " + newAmount);
            amount = newAmount;

            image.DOFade(amount, transitionDuration);

            var v = 1 + ((1 - amount) / 4);
            rectTransform.DOScale(new Vector3(v, v, v), transitionDuration);
        }
    }
}
