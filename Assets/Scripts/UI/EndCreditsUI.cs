using System;
using System.Collections;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class EndCreditsUI : MonoBehaviour
    {
        public Text endGameText;
        public string[] endGameStrings;
        
        public Text creditsText;

        private CanvasGroup creditsCG;

        [SerializeField]
        private GameObject skipIndicator = null;

        private string nextScene = "EndScoreboard";

        private void Start()
        {
            creditsCG = creditsText.GetComponent<CanvasGroup>();
            
            creditsCG.DOFade(0f, 0f);
            
            endGameText.DOFade(0f, 0f);
            // endGameText.rectTransform.DOScale(new Vector3(0f, 0f, 0f), 0f);

            StartCoroutine(EndGameText());
        }

        public void OnAny()
        {
            skipIndicator.SetActive(true);
        }

        public void OnInteract()
        {
            SceneManager.LoadScene(nextScene);
        }

        public void OnPause()
        {
            SceneManager.LoadScene(nextScene);
        }

        private IEnumerator EndGameText()
        {
            foreach (var str in endGameStrings)
            {
                endGameText.DOFade(0f, 0.1f);
                yield return new WaitForSeconds(0.2f);
                
                // endGameText.rectTransform.DOScale(new Vector3(0f, 0f, 0f), 0f);
                endGameText.text = str;
                
                // endGameText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuint);
                endGameText.DOFade(1f, 0.5f).SetEase(Ease.OutQuint);
                yield return new WaitForSeconds(2);
            }
            yield return new WaitForSeconds(1);
            endGameText.DOFade(0f, 0.1f);
            yield return ScrollCredits();
        }

        private IEnumerator ScrollCredits()
        {
            creditsCG.DOFade(1f, 5f).WaitForCompletion();
            yield return creditsText.rectTransform.DOAnchorPosY(creditsText.rectTransform.sizeDelta.y + (Screen.height/2), 25f)
                .SetEase(Ease.Linear).WaitForCompletion();

            SceneManager.LoadScene(nextScene);
        }
    }
}
