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

            startingElevator.active = !f;
            finishElevator.active = f;
            //finishElevator.Open();

            player.position = f ? startingPosA.position : startingPosB.position;

            GenerateRoom.Current.Generate();

            ObjectGeneration.Current.GenerateObjects();

            EnemySpawner.Current.StartSpawning();
        }
    }
}
