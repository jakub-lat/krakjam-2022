using System;
using System.Collections.Generic;
using System.Linq;
using Cyberultimate.Unity;
using Player;
using Scoreboard;
using UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public enum GameMode
    {
        Easy,
        Normal,
        Hard
    }

    [Serializable]
    public class EnemyDifficultyMultiplier
    {
        public AnimationCurve health;
        public AnimationCurve damage;
        public AnimationCurve speed;
        public AnimationCurve attackRange;
        public AnimationCurve attackSpeed;
    }
    
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public GameMode GameMode => (GameMode)PlayerPrefs.GetInt("GameMode");

        public int CurrentLevel { get; private set; }

        public Transform startingPosA;
        public Transform startingPosB;
        public Elevator startingElevator;
        public Elevator finishElevator;
        public Transform player;
        public Transform cameraHolder;
        public Transform bossRoomSpawnPoint;

        [Header("Level settings")]
        public int levelCount;
        public List<string> levelDescriptions;

        [Header("Game balance")] 
        [SerializeField] private AnimationCurve difficultyCurveEasy;
        [SerializeField] private AnimationCurve difficultyCurveNormal;
        [SerializeField] private AnimationCurve difficultyCurveHard;

        public EnemyDifficultyMultiplier shootingEnemy;
        public EnemyDifficultyMultiplier meleeEnemy;

        [Header("Score counting")] 
        public int killScore;
        public int headshotScore;

        public AnimationCurve DifficultyCurve => GameMode == GameMode.Easy ? difficultyCurveEasy :
            GameMode == GameMode.Normal ? difficultyCurveNormal : difficultyCurveHard;
        public float Difficulty { get { return DifficultyCurve.Evaluate(CurrentLevel / levelCount); } }

        private int score;
        public int Score
        {
            get => score;
            set
            {
                int delta = value - score;
                score = value;
                GameScoreboard.Current.levelData.score = score;
                ScoreUI.Current.SetScore(score, delta);
            }
        }
        
        private int width, height;
        private float spaceX, spaceZ;

        protected override void Awake()
        {
            base.Awake();
            
            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            width = GenerateRoom.Current.width;
            height = GenerateRoom.Current.height;
            spaceX = GenerateRoom.Current.spaceX;
            spaceZ = GenerateRoom.Current.spaceZ;
            
            (startingPosA, startingPosB) = (startingPosB, startingPosA);

            GenerateRoom.Current.PreRenderFloors(levelCount);

            NextLevel();
            player.position = startingPosA.position;
            cameraHolder.localRotation = startingPosA.localRotation;
            if (!Scoreboard.GameScoreboard.Current.runDataSet)
            {
                Scoreboard.GameScoreboard.Current.NewRun();
            }

            Invoke(nameof(DisplayObjective), 2f);
        }

        void DisplayObjective()
        {
            ObjectivesUI.Current.SetObjective("MISSION: VISIT THE BOSS", "FIND THE NEXT ELEVATOR AND HEAD TO THE TOP FLOOR");
        }

        public void NextLevel()
        {
            Score = 0;
            CurrentLevel++;
            
            GameScoreboard.Current.ResetLevelData();  

            if (CurrentLevel-1 >= levelCount)
            {
                BossLevel();
                return;
            }


            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            (startingPosA, startingPosB) = (startingPosB, startingPosA);

            GenerateLevel();
            Landscape.Current.SetElevation(CurrentLevel);

            // startingElevator.Open();
            startingElevator.Active = false;
            finishElevator.Active = true;

            /* if (CurrentLevel == 1)
             {
                 player.position = startingPosA.position;
                 cameraHolder.localRotation = startingPosA.localRotation;
                 Debug.Log("setpos");
             }*/
        }

        public void BossLevel()
        {
            
        }

        private void GenerateLevel()
        {
            Vector3 newElevatorPos = new Vector3(finishElevator.transform.position.x,
                finishElevator.transform.position.y, UnityEngine.Random.Range(0, height * 2) * spaceZ);
            finishElevator.transform.position = newElevatorPos;

            finishElevator.SetDoorNavSurface(false); //enemies cant walk through door
            startingElevator.SetDoorNavSurface(true);

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
