using System;
using Cyberultimate.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreUI : MonoSingleton<ScoreUI>
    {
        [SerializeField] private Text scoreText;

        public void SetScore(int score)
        {
            scoreText.text = $"SCOrE: {score}";
        }
    }
}
