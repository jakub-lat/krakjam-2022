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

        public Transform startingPos;
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

            NextLevel();
            player.position = startingPos.position;
            cameraHolder.localRotation = startingPos.localRotation;
        }

        public void NextLevel()
        {
            CurrentLevel++;

            (startingElevator, finishElevator) = (finishElevator, startingElevator);
            
            startingElevator.Open();
            startingElevator.active = false;
            finishElevator.active = true;

            var pos = new Vector3(finishElevator.transform.position.x, finishElevator.transform.position.y, UnityEngine.Random.Range(0, height*2) * spaceZ);

            finishElevator.transform.position = pos;

            GenerateRoom.Current.transform.KillAllChildren();
            GenerateRoom.Current.Generate();

            ObjectGeneration.Current.GenerateObjects();

            EnemySpawner.Current.transform.KillAllChildren();
            EnemySpawner.Current.StartSpawning();
      
        }
    }
}
