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

        private void Start()
        {
            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            width = GenerateRoom.Current.width;
            height = GenerateRoom.Current.height;
            spaceX = GenerateRoom.Current.spaceX;
            spaceZ = GenerateRoom.Current.spaceZ;
            
            if (startingPosA != null)
            {
                player.position = startingPosA.position;
                cameraHolder.localRotation = startingPosA.localRotation;
            }

            NextLevel();
            if (!Scoreboard.GameScoreboard.Current.runDataSet)
            {
                Scoreboard.GameScoreboard.Current.NewRun();
            }
        }

        public void NextLevel()
        {
            CurrentLevel++;

            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            
            GenerateLevel();
            
            // startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;
        }

        private void GenerateLevel()
        {
            Vector3 newElevatorPos = new Vector3(finishElevator.transform.position.x, finishElevator.transform.position.y, UnityEngine.Random.Range(0, height * 2) * spaceZ);
            finishElevator.transform.position = newElevatorPos;

            var levelDifficulty = difficulty.Evaluate(CurrentLevel);
            GenerateRoom.Current.transform.KillAllChildren();
            ObjectGeneration.Current.ClearObjects();
            GenerateRoom.Current.Generate(CurrentLevel - 1);
            GenerateRoom.Current.RefreshMesh();
            ObjectGeneration.Current.GenerateObjects();

            
            finishElevator.elevatorRemover.Remove();
            startingElevator.elevatorRemover.Remove();

            EnemySpawner.Current.KillAll();
            EnemySpawner.Current.StartSpawning();

            GenerateRoom.Current.RefreshMesh();
        }
    }
}
