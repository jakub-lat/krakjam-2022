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
            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            NextLevel();
        }

        public void NextLevel()
        {
            CurrentLevel++;
//finishElevator.Open();

            // player.position = f ? startingPosA.position : startingPosB.position;

            GenerateRoom.Current.transform.KillAllChildren();
            GenerateRoom.Current.Generate();

            ObjectGeneration.Current.GenerateObjects();

            EnemySpawner.Current.StartSpawning();

            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            
            startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;
        }
    }
}
