using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scoreboard;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Game
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private Transform bossElevator;
        [SerializeField] private Transform playerSpawn;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private GameObject exitBlock;
        [SerializeField] private NavMeshLink leftDoor, rightDoor;
        [SerializeField] private Transform doorsLeft, doorsRight;
        [SerializeField] private Vector3 doorsLeftOpenLocalPos, doorsRightOpenLocalPos;
        [SerializeField] private float animDuration, closeDelay;

        [SerializeField] private MeshRenderer statusLight;
        [SerializeField] private Material statusLightGreenMaterial;
        [SerializeField] private Material statusLightRedMaterial;

        [SerializeField] private Text floorText;
        [SerializeField] private Vector2 floorTextEndPos;
        [SerializeField] private float startMovingDelay;

        [SerializeField] private Transform illusionObj;
        [SerializeField] private float illusionStartY;
        [SerializeField] private float illusionEndY;

        [SerializeField] private Transform floorButtonContainer;
        [SerializeField] private ElevatorButton floorButtonPrefab;
        private List<ElevatorButton> _floorButtons;

        private Vector2 floorTextStartPos;

        private Vector3 doorsLeftClosedLocalPos, doorsRightClosedLocalPos;

        [SerializeField]
        private AudioSource music;

        [SerializeField]
        private AudioSource doorOpen;

        [SerializeField]
        private AudioClip doorOpenClip;
        
        public bool openOnStart;

        private bool active;
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                statusLight.material = active ? statusLightGreenMaterial : statusLightRedMaterial;
            }
        }

        [SerializeField]
        private float startWaitTime = 5f;

        [SerializeField] private float musicTransition;

        public ElevatorRemover elevatorRemover;
        
        private void Start()
        {
            music = GetComponent<AudioSource>();
            
            exitBlock.SetActive(false);
            doorsLeftClosedLocalPos = doorsLeft.localPosition;
            doorsRightClosedLocalPos = doorsRight.localPosition;

            var lvlCnt = LevelManager.Current.levelCount;
            _floorButtons = new List<ElevatorButton>();
            for (var i = lvlCnt; i >= 1 ; i--)
            {
                var btn = Instantiate(floorButtonPrefab, floorButtonContainer);
                btn.Init(i, LevelManager.Current.levelDescriptions.ElementAtOrDefault(i-1) ?? string.Empty);
                _floorButtons.Add(btn);
            }

            _floorButtons.Reverse();

            UpdateFloorText();

            floorTextStartPos = floorText.rectTransform.anchoredPosition;

            illusionObj.gameObject.SetActive(false);
            
            if (openOnStart)
            {
                GetComponent<ElevatorScoreboard>().Hide();
                music.Play();
                Invoke(nameof(UpdateFloorText), 0.5f);
                Invoke(nameof(Open), startWaitTime);
                MovingUpIllusion(startWaitTime);
                exitBlock.SetActive(false);
            }

            if (LevelManager.Current.levelCount is 0)
            {
                GoToBoss();
            }

            // Debug.Log($"start: current level={LevelManager.Current.CurrentLevel}");
        }

        private void Print(string text)
        {
            // Debug.Log($"{gameObject.name} (active: {active}): {text}");
        }

        public void OnTriggerEnter(Collider other)
        {
            Print("trigger enter");
            if (!Active) return;

            if (other.gameObject.CompareTag("Player"))
            {
                Open();
                UpdateFloorText();
                if (LevelManager.Current.CurrentLevel > 0)
                {
                    RunScoreboardTasks();
                }
            }
        }
        public void SetDoorNavSurface(bool walkable)
        {
            leftDoor.enabled = walkable;
            rightDoor.enabled = walkable;
        }

        private async void RunScoreboardTasks()
        {
            Debug.Log("posting level data... " + LevelManager.Current.CurrentLevel);
            GameScoreboard.Current.levelData.level = LevelManager.Current.CurrentLevel;
            await GameScoreboard.Current.PostLevelData();
            await GetComponent<ElevatorScoreboard>().Show(LevelManager.Current.CurrentLevel);
        }

        public void OnTriggerExit(Collider other)
        {
            Print("trigger exit");
            if (!Active && other.gameObject.CompareTag("Player"))
            {
                Invoke(nameof(Close), 0.5f);
            }
        }

        public void Use()
        {
            if (!Active) return;
            
            Print("use");

            Active = false;

            music.Play();
            music.DOFade(1, musicTransition);
            GameMusic.Current.FadeOut(musicTransition);

            exitBlock.SetActive(true);
            
            Close().OnComplete(() =>
            {
                Print("closed");

                MovingUpIllusion(startMovingDelay);
                
                Invoke(nameof(NextLevel), startMovingDelay / 2);
                Invoke(nameof(UpdateFloorText), (startMovingDelay / 2) + 0.1f);
                
                floorText.rectTransform.DOAnchorPos(floorTextEndPos, animDuration)
                    .SetEase(Ease.OutCirc)
                    .SetDelay(startMovingDelay)
                    .OnComplete(() =>
                    {
                        foreach (var x in _floorButtons)
                        {
                            x.Unselect();
                        }
                        
                        Print("floor text completed - opening");
                        Open();
                    }).SetLink(this.gameObject);
            }).SetLink(this.gameObject);
        }

        private void GoToBoss()
        {
            Debug.Log("BOSSSSS");
            
            // playerTransform.parent = transform;

            transform.DOMove(bossElevator.position, 0f);
            var cc = playerTransform.GetComponent<CharacterController>();
            cc.enabled = false;
            playerTransform.position = bossElevator.position;
            cc.enabled = true;

            Open();
            
            var diff = bossElevator.localEulerAngles - transform.localEulerAngles;
            transform.localEulerAngles += diff;

            playerTransform.parent = transform.parent.parent;
            cameraHolder.localEulerAngles += diff;
        }
        
        private void NextLevel()
        {
            LevelManager.Current.NextLevel();
            
            Debug.Log($"current level: {LevelManager.Current.CurrentLevel}, count: {LevelManager.Current.levelCount}");

            if (LevelManager.Current.CurrentLevel-1 >= LevelManager.Current.levelCount) //last lvl
            {
                GoToBoss();
            }
        }
        
        private void UpdateFloorText()
        {
            var lvl = LevelManager.Current.CurrentLevel;
            floorText.text = $"{lvl + 1}\n{lvl}";
            
            // Debug.Log($"Updating floor text - current level = {LevelManager.Current.CurrentLevel}");
            foreach (var x in _floorButtons)
            {
                x.Unselect();
            }
            // Debug.Log(lvl);
            _floorButtons[lvl-1].Select();
        }

        public Tween Open()
        {
            // Print("open");
            foreach (var x in _floorButtons)
            {
                x.Unselect();
            }

            music.DOFade(0, musicTransition);
            GameMusic.Current.FadeIn(musicTransition);
            // Debug.Log("MUSIC IS PLAYING " + GameMusic.Current.audioSource.isPlaying);
            
            doorsLeft.DOLocalMove(doorsLeftOpenLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint).OnComplete(() =>
                {
                    exitBlock.SetActive(false);
                });

            doorOpen.PlayOneShot(doorOpenClip);
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

            doorOpen.PlayOneShot(doorOpenClip);
            return doorsRight.DOLocalMove(doorsRightClosedLocalPos, animDuration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutQuint)
                .SetDelay(closeDelay);
        }
        
        public void MovingUpIllusion(float duration) {
            illusionObj.gameObject.SetActive(true);
            illusionObj.DOLocalMoveY(illusionStartY, 0);
            
            Debug.Log("duration: "+ duration);

            illusionObj.DOLocalMoveY(illusionEndY, duration).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                illusionObj.gameObject.SetActive(false);
            });
        }
    }
}
