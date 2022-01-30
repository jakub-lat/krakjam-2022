using Cyberultimate.Unity;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreUI : MonoSingleton<ScoreUI>
    {
        [SerializeField] private Text scoreText;

        [SerializeField]
        private Vector3 scaledUpText;
        private Vector3 normScaleText;

        [SerializeField]
        private Ease scaleEase;

        [SerializeField]
        private Ease scaleOutEase;

        private float score;
        private float displayScore;
        private Coroutine scoreUpdate;

        protected void Start()
        {
            normScaleText = scoreText.transform.localScale;
        }

        // delta -> how much to add
        // score -> actual score after adding
        public void SetScore(int score, int delta)
        {
            this.score = score;


            StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            scoreText.transform.DOScale(scaledUpText, 1).SetEase(scaleEase).SetLink(this.gameObject);
            yield return ScoreUpdater();
            scoreText.transform.DOScale(normScaleText, 1).SetEase(scaleOutEase).SetLink(this.gameObject);
        }

        private IEnumerator ScoreUpdater()
        {

            while (displayScore < score)
            {
                displayScore++;
                scoreText.text = $"SCOrE: {displayScore}";
                yield return new WaitForSeconds(0.1f);
            }

            scoreUpdate = null;
        }
    }
}
