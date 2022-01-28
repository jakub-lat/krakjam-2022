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

public class PauseObject : MonoSingleton<PauseObject>
{
    [SerializeField]
    private float openScoreDuration = 2;

    [SerializeField]
    private Ease openScoreEase = Ease.OutElastic;

    [SerializeField]
    private Button scoreboardBtn = null;

    [SerializeField]
    private GameObject options = null;

    private float savedHeight;
    private float savedPositionY;

    [SerializeField] private Text statsText;

    [SerializeField]
    private Button optionsBtn = null;
    private Text optionsTxt = null;

    protected void Start()
    {
        optionsTxt = optionsBtn.transform.GetChild(0).GetComponent<Text>();
    }

    public void OnOpen()
    {
        var data = Scoreboard.GameScoreboard.Current.levelData;
        statsText.text = string.Join("\n", new[] { data.level, data.score, data.kills, data.headshots, data.deaths }.Select(x => x.ToString()));
    }

    public void OnResume()
    {
        PauseManager.Current.SwitchPause();
    }

    public void OnQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRevive()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OnOutOptions()
    {
        options.SetActive(false);
        optionsBtn.onClick.RemoveAllListeners();
        optionsBtn.onClick.AddListener(OnOptions);
        optionsTxt.text = "Options";
    }

    public void OnOptions()
    {
        options.SetActive(true);
        optionsBtn.onClick.RemoveAllListeners();
        optionsBtn.onClick.AddListener(OnOutOptions);
        optionsTxt.text = "Back";
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
