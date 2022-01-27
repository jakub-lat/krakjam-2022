using LetterBattle.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PauseObject : MonoBehaviour
{
    [SerializeField]
    private float openScoreDuration = 2;

    [SerializeField]
    private Ease openScoreEase = Ease.OutElastic;

    public void OnResume()
    {
        PauseManager.Current.SwitchPause();
    }

    public void OnQuit()
    {
        TimeScaling.Status.Unregister(PauseManager.Current.LastObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRevive()
    {
        TimeScaling.Status.Unregister(PauseManager.Current.LastObject);
        SceneManager.LoadScene("Game");
    }

    public void OpenScoreboard(Button btnClicked)
    {
        btnClicked.onClick.RemoveAllListeners();
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, btnClicked.targetGraphic.transform.DOMoveY(360, openScoreDuration)).SetEase(openScoreEase);
        seq.Insert(0, btnClicked.targetGraphic.rectTransform.DOSizeDelta(
            new Vector2(btnClicked.targetGraphic.rectTransform.sizeDelta.x, 2400), openScoreDuration).SetEase(openScoreEase));

        seq.SetLink(this.gameObject).SetUpdate(true);
    }
}
