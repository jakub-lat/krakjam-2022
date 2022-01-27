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

        private void Start()
        {
            exitBlock.SetActive(false);
            doorsLeftClosedLocalPos = doorsLeft.localPosition;
            doorsRightClosedLocalPos = doorsRight.localPosition;

            UpdateFloorText();

            floorTextStartPos = floorText.rectTransform.anchoredPosition;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Open();
            }
        }

        public void Use()
        {
            UpdateFloorText();
            exitBlock.SetActive(true);
            Close().OnComplete(() =>
            {
                floorText.rectTransform.DOAnchorPos(floorTextEndPos, animDuration)
                    .SetEase(Ease.OutCirc)
                    .SetDelay(startMovingDelay)
                    .OnComplete(() => { LevelManager.Current.NextLevel(); });
            });
        }

        private void UpdateFloorText()
        {
            var lvl = LevelManager.Current.CurrentLevel;
            floorText.text = $"{lvl + 2}\n{lvl + 1}";
        }

        private void Open()
        {
            doorsLeft.DOLocalMove(doorsLeftOpenLocalPos, animDuration)
                .SetEase(Ease.InOutQuint);
            doorsRight.DOLocalMove(doorsRightOpenLocalPos, animDuration)
                .SetEase(Ease.InOutQuint);
        }

        private Tween Close()
        {
            doorsLeft.DOLocalMove(doorsLeftClosedLocalPos, animDuration)
                .SetEase(Ease.InOutQuint).SetDelay(closeDelay);
            return doorsRight.DOLocalMove(doorsRightClosedLocalPos, animDuration)
                .SetEase(Ease.InOutQuint).SetDelay(closeDelay);
        }
    }
}
