using System;
using DG.Tweening;
using Scoreboard;
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

        private AudioSource music;
        
        public bool openOnStart;
        public bool active;

        [SerializeField]
        private float startWaitTime = 5f;

        public ElevatorRemover elevatorRemover;
        
        private void Start()
        {
            music = GetComponent<AudioSource>();
            
            exitBlock.SetActive(false);
            doorsLeftClosedLocalPos = doorsLeft.localPosition;
            doorsRightClosedLocalPos = doorsRight.localPosition;

            UpdateFloorText();

            floorTextStartPos = floorText.rectTransform.anchoredPosition;

            if (openOnStart)
            {
                GetComponent<ElevatorScoreboard>().Hide();
                music.Play();
                Invoke(nameof(Open), startWaitTime);
                exitBlock.SetActive(false);
            }
        }

        private void Print(string text)
        {
            Debug.Log($"{gameObject.name} (active: {active}): {text}");
        }

        public void OnTriggerEnter(Collider other)
        {
            //Print("trigger enter");
            if (!active) return;

            if (other.gameObject.CompareTag("Player"))
            {
                Open();
                if (LevelManager.Current.CurrentLevel > 0)
                {
                    RunScoreboardTasks();
                }
            }
        }

        private async void RunScoreboardTasks()
        {
            await GameScoreboard.Current.PostLevelData();
            await GetComponent<ElevatorScoreboard>().Show(LevelManager.Current.CurrentLevel);
        }

        public void OnTriggerExit(Collider other)
        {
            //Print("trigger exit");
            if (!active && other.gameObject.CompareTag("Player"))
            {
                Invoke(nameof(Close), 0.5f);
            }
        }

        public void Use()
        {
            if (!active) return;
            
            Print("use");

            active = false;
            
            music.Play();
            music.DOFade(1, closeDelay);

            UpdateFloorText();
            exitBlock.SetActive(true);
            Close().OnComplete(() =>
            {
                Print("closed");
                LevelManager.Current.NextLevel();
                floorText.rectTransform.DOAnchorPos(floorTextEndPos, animDuration)
                    .SetEase(Ease.OutCirc)
                    .SetDelay(startMovingDelay)
                    .OnComplete(() =>
                    {
                        Print("floor text completed - opening");
                        Open();
                    }).SetLink(this.gameObject);
            }).SetLink(this.gameObject);
        }

        private void UpdateFloorText()
        {
            var lvl = LevelManager.Current.CurrentLevel;
            floorText.text = $"{lvl + 1}\n{lvl}";
        }

        public Tween Open()
        {
            Print("open");
            
            music.DOFade(0, closeDelay).OnComplete(() => music.Stop());
            
            doorsLeft.DOLocalMove(doorsLeftOpenLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint).OnComplete(() =>
                {
                    exitBlock.SetActive(false);
                });

            return doorsRight.DOLocalMove(doorsRightOpenLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint);
        }

        public Tween Close()
        {
            Print("close");
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
