using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public Transform cameraHolder;

        private void Start()
        {
            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            NextLevel();
            player.position = startingPosA.position;
            cameraHolder.localRotation = startingPosA.localRotation;
        }

        public void NextLevel()
        {
            CurrentLevel++;

            GenerateLevel();
            
            EnemySpawner.Current.StartSpawning();

            (startingElevator, finishElevator) = (finishElevator, startingElevator);

            startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;
        }

        private void GenerateLevel()
        {
            GenerateRoom.Current.transform.KillAllChildren();
            GenerateRoom.Current.Generate();
            ObjectGeneration.Current.GenerateObjects();
            Debug.Log("done");
        }
    }
}
