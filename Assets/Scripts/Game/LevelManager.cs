using System;
using System.Linq;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private int levelCount;
        [SerializeField] private AnimationCurve difficulty;

        public int CurrentLevel { get; private set; }

        public Transform startingPosA;
        public Transform startingPosB;
        public Elevator startingElevator;
        public Elevator finishElevator;
        public Transform player;

        private void Start()
        {
            NextLevel();
        }

        bool f = true;
        public void NextLevel()
        {
            CurrentLevel++;

            startingElevator.enabled = !f;
            finishElevator.enabled = f;

            player.position = f ? startingPosA.position : startingPosB.position;
            player.rotation = f ? startingPosA.rotation : startingPosB.rotation;

            GenerateRoom.Current.Generate();

            ObjectGeneration.Current.GenerateObjects();
        }
    }
}
