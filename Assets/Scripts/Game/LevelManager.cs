using System;
using System.Collections.Generic;
using System.Linq;
using Cyberultimate.Unity;
using Player;
using Scoreboard;
using UnityEngine;

namespace Game
{
    public enum GameMode
    {
        Easy, Normal, Hard
    }
    
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private int levelCount;
        [SerializeField] private AnimationCurve difficulty;

        public GameMode GameMode => (GameMode)PlayerPrefs.GetInt("GameMode");

        public int CurrentLevel { get; private set; }

        public Transform startingPosA;
        public Transform startingPosB;
        public Elevator startingElevator;
        public Elevator finishElevator;
        public Transform player;
        public Transform cameraHolder;
        public Transform bossRoomSpawnPoint;

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

            (startingPosA, startingPosB) = (startingPosB, startingPosA);

            GenerateRoom.Current.PreRenderFloors(levelCount);

            NextLevel();
            if (!Scoreboard.GameScoreboard.Current.runDataSet)
            {
                Scoreboard.GameScoreboard.Current.NewRun();
            }
        }

        public void NextLevel()
        {
            if (CurrentLevel == levelCount)
            {
                BossLevel();
                return;
            }

            CurrentLevel++;

            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            (startingPosA, startingPosB) = (startingPosB, startingPosA);

            GenerateLevel();
            
            // startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;
        }
        public void BossLevel()
        {
            Debug.Log("bosz");
            player.transform.position = bossRoomSpawnPoint.position;
            Boss.Current.StartBattle();
        }

        private void GenerateLevel()
        {
            Vector3 newElevatorPos = new Vector3(finishElevator.transform.position.x, finishElevator.transform.position.y, UnityEngine.Random.Range(0, height * 2) * spaceZ);
            finishElevator.transform.position = newElevatorPos;

            finishElevator.SetDoorNavSurface(false); //enemies cant walk through door
            startingElevator.SetDoorNavSurface(true);

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
