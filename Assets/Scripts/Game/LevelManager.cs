using System;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private int levelCount;
        [SerializeField] private AnimationCurve difficulty;

        public int CurrentLevel { get; private set; }

        private void Start()
        {
            LevelGenerator.Current.GenerateLevel(difficulty.Evaluate(CurrentLevel / levelCount));
        }

        public void NextLevel()
        {
            CurrentLevel++;
            LevelGenerator.Current.GenerateLevel(difficulty.Evaluate(CurrentLevel / levelCount));
        }
    }
}
