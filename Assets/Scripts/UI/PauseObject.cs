using System;
using LetterBattle.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cyberultimate.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Scoreboard;
using UnityEngine.Serialization;

public class PauseObject : MonoSingleton<PauseObject>
{
    [SerializeField] private float openScoreDuration = 2;

    [SerializeField] private Ease openScoreEase = Ease.OutElastic;

    [SerializeField] private Button scoreboardBtn = null;

    [SerializeField] private CanvasGroup options = null;

    private float savedHeight;
    private float savedPositionY;

    [FormerlySerializedAs("statsText")] [SerializeField]
    private Text pauseStatsText;

    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text gameOverStatsText;

    [SerializeField] private Button optionsBtn = null;
    private Text optionsTxt = null;

    [SerializeField] private Image optionsOverlay;


    protected void Start()
    {
        optionsTxt = optionsBtn.transform.GetChild(0).GetComponent<Text>();
        OnOutOptions();
    }

    public void OnOpen()
    {
        var data = Scoreboard.GameScoreboard.Current.levelData;
        pauseStatsText.text = string.Join("\n",
            new[] { data.level, data.score, data.kills, data.headshots, data.deaths }.Select(x => x.ToString()));
    }

    public void OnGameOver()
    {
        var run = GameScoreboard.Current.GetCalculatedRunData();
        gameOverScoreText.text = run.score.ToString();
        gameOverStatsText.text = string.Join("\n",
            new object[]
            {
                run.kills, run.headshots, run.deaths,
                TimeSpan.FromSeconds(run.endTime - run.startTime).ToString(@"mm\:ss")
            }.Select(x => x.ToString())
        );
    }

    public void OnResume()
    {
        PauseManager.Current.SwitchPause();
    }

    public void OnQuit()
    {
        Time.timeScale = 1;
        SceneLoader.LoadScene("MainMenu");
    }

    public void OnRevive()
    {
        Time.timeScale = 1;
        SceneLoader.LoadScene("Game");
    }

    public void OnOutOptions()
    {
        // optionsOverlay.DOFade(0, 0.5f);
        // options.DOFade(0, 0.3f);
        // options.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuint).OnComplete(() =>
        // {
        //     options.gameObject.SetActive(false);
        // });
        // todo why tweens not working
        
        var c = optionsOverlay.color;
        c.a = 0;
        optionsOverlay.color = c;
        
        options.gameObject.SetActive(true);
        options.transform.localScale = Vector3.zero;
        options.alpha = 0;
    }

    public void OnOptions()
    {
        // optionsOverlay.DOFade(1, 0.5f);
        // options.DOFade(1, 0.5f);
        // options.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);
        // todo why tweens not working
        
        var c = optionsOverlay.color;
        c.a = 1;
        optionsOverlay.color = c;
        
        options.gameObject.SetActive(true);
        options.transform.localScale = Vector3.one;
        options.alpha = 1;
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

        seq.SetLink(this.gameObject).SetUpdate(true).SetEase(openScoreEase)
            .OnComplete(() => scoreboardBtn.onClick.AddListener(() => OpenScoreboard()));
    }
}
