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

                Toggle(true);
                // LevelManager.Current.NextLevel();
            }
        }

        public void Use()
        {
            exitBlock.SetActive(true);
            Toggle(false);
        }
        
        private void Toggle(bool isOpen)
        {
            doorsLeft.DOLocalMove(isOpen ? doorsLeftOpenLocalPos : doorsLeftClosedLocalPos, animDuration)
                .SetEase(Ease.InOutQuint).SetDelay(isOpen ? 0 : closeDelay);
            doorsRight.DOLocalMove(isOpen ? doorsRightOpenLocalPos : doorsRightClosedLocalPos, animDuration)
                .SetEase(Ease.InOutQuint).SetDelay(isOpen ? 0 : closeDelay);
        }
    }
}
