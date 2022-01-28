using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private GameObject exitBlock;
        [SerializeField] private Transform doorsLeft, doorsRight;
        [SerializeField] private Vector3 doorsLeftOpenLocalPos, doorsRightOpenLocalPos;
        [SerializeField] private float animDuration, closeDelay;

        [SerializeField] private Text floorText;
        [SerializeField] private Vector2 floorTextEndPos;
        [SerializeField] private float startMovingDelay;
        private Vector2 floorTextStartPos;

        private Vector3 doorsLeftClosedLocalPos, doorsRightClosedLocalPos;

        public bool openOnStart;
        public bool active;
        
        private void Start()
        {
            exitBlock.SetActive(false);
            doorsLeftClosedLocalPos = doorsLeft.localPosition;
            doorsRightClosedLocalPos = doorsRight.localPosition;

            UpdateFloorText();

            floorTextStartPos = floorText.rectTransform.anchoredPosition;

            if (openOnStart)
            {
                Open();
                exitBlock.SetActive(false);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!active) return;

            if (other.gameObject.CompareTag("Player"))
            {
                Open();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (!active && other.gameObject.CompareTag("Player"))
            {
                Invoke(nameof(Close), 0.5f);
            }
        }

        public void Use()
        {
            if (!active) return;
            UpdateFloorText();
            exitBlock.SetActive(true);
            Close().OnComplete(() =>
            {
                LevelManager.Current.NextLevel();
                floorText.rectTransform.DOAnchorPos(floorTextEndPos, animDuration)
                    .SetEase(Ease.OutCirc)
                    .SetDelay(startMovingDelay)
                    .OnComplete(() =>
                    {
                        Open();
                    });
            });
        }

        private void UpdateFloorText()
        {
            var lvl = LevelManager.Current.CurrentLevel;
            floorText.text = $"{lvl + 1}\n{lvl}";
        }

        public Tween Open()
        {
            doorsLeft.DOLocalMove(doorsLeftOpenLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint);
            return doorsRight.DOLocalMove(doorsRightOpenLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint);
        }

        public Tween Close()
        {
            doorsLeft.DOLocalMove(doorsLeftClosedLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint)
                .SetDelay(closeDelay);
            return doorsRight.DOLocalMove(doorsRightClosedLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint)
                .SetDelay(closeDelay);
        }
    }
}
