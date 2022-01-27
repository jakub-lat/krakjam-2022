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

    [SerializeField]
    private Button scoreboardBtn = null;

    private float savedHeight;
    private float savedPositionY;

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

    public void OpenScoreboard()
    {
        savedHeight = scoreboardBtn.targetGraphic.rectTransform.sizeDelta.y;
        savedPositionY = scoreboardBtn.targetGraphic.transform.position.y;

        scoreboardBtn.onClick.RemoveAllListeners();
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, scoreboardBtn.targetGraphic.transform.DOMoveY(360, openScoreDuration));
        seq.Insert(0, scoreboardBtn.targetGraphic.rectTransform.DOSizeDelta(
            new Vector2(scoreboardBtn.targetGraphic.rectTransform.sizeDelta.x, 2400), openScoreDuration));

        seq.SetLink(this.gameObject).SetUpdate(true).SetEase(openScoreEase);
    }

    public void CloseScoreboard()
    {
        Sequence seq = DOTween.Sequence();
        seq.Insert(0, scoreboardBtn.targetGraphic.transform.DOMoveY(savedPositionY, openScoreDuration));
        seq.Insert(0, scoreboardBtn.targetGraphic.rectTransform.DOSizeDelta(
            new Vector2(scoreboardBtn.targetGraphic.rectTransform.sizeDelta.x, savedHeight), openScoreDuration));

        seq.SetLink(this.gameObject).SetUpdate(true).SetEase(openScoreEase).OnComplete(() => scoreboardBtn.onClick.AddListener(() => OpenScoreboard()));
    }
}
