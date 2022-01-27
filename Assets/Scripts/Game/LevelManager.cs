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

        private void Start()
        {
            // LevelGenerator.Current.GenerateLevel(difficulty.Evaluate(CurrentLevel / levelCount));
        }

        public void NextLevel()
        {
            CurrentLevel++;

            foreach (var child in transform.GetChildren())
            {
                Destroy(child.gameObject);
            }

            // LevelGenerator.Current.GenerateLevel(difficulty.Evaluate(CurrentLevel / levelCount));
        }
    }
}
