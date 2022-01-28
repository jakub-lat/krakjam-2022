using System;
using System.Collections.Generic;
using System.Linq;
using Cyberultimate.Unity;
using Player;
using Scoreboard;
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

        private int width, height;
        private float spaceX, spaceZ;
        private int elevator1Z, elevator2Z;

        private void Start()
        {
            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            width = GenerateRoom.Current.width;
            height = GenerateRoom.Current.height;
            spaceX = GenerateRoom.Current.spaceX;
            spaceZ = GenerateRoom.Current.spaceZ;

            elevator1Z = (int)(finishElevator.transform.position.z / spaceZ);
            
            if (startingPosA != null)
            {
                player.position = startingPosA.position;
                cameraHolder.localRotation = startingPosA.localRotation;
            }

            NextLevel();

            Scoreboard.GameScoreboard.Current.NewRun();
        }

        public void NextLevel()
        {
            if (CurrentLevel > 0)
            {
                Scoreboard.GameScoreboard.Current.PostLevelData();
            }

            CurrentLevel++;

            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            
            GenerateLevel();

            EnemySpawner.Current.transform.KillAllChildren();
            EnemySpawner.Current.StartSpawning(); 

            // startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;
        }

        private void GenerateLevel()
        {
            elevator2Z = UnityEngine.Random.Range(0, height * 2);
            Vector3 newElevatorPos = new Vector3(finishElevator.transform.position.x, finishElevator.transform.position.y,elevator2Z*spaceZ);

            finishElevator.transform.position = newElevatorPos;

            GenerateRoom.Current.transform.KillAllChildren();
            GenerateRoom.Current.Generate(elevator1Z, elevator2Z);
            ObjectGeneration.Current.GenerateObjects();
            Debug.Log("done");
        }
    }
}
