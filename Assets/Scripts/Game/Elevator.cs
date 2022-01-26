using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private GameObject exitBlock;
        [SerializeField] private Transform doorsLeft, doorsRight;
        [SerializeField] private Vector3 doorsLeftOpenLocalPos, doorsRightOpenLocalPos;
        [SerializeField] private float animDuration, closeDelay;

        private Vector3 doorsLeftClosedLocalPos, doorsRightClosedLocalPos;

        private void Start()
        {
            exitBlock.SetActive(false);
            doorsLeftClosedLocalPos = doorsLeft.localPosition;
            doorsRightClosedLocalPos = doorsRight.localPosition;
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
            exitBlock.SetActive(true);
            Close().OnComplete(() => { LevelManager.Current.NextLevel(); });
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
