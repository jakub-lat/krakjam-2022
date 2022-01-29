using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform logo = null;

    [SerializeField]
    private LineRenderer lineRender = null;

    [SerializeField]
    private Text pressKey = null;
    private float basePositionLogo;

    [SerializeField]
    private Image background;

    [SerializeField]
    private CanvasGroup buttonsAndStuff = null;

    [SerializeField] private GameObject gameModePopup;
    [SerializeField] private RectTransform gameModePanel;
    [SerializeField] private Image gameModeOverlay;

    [SerializeField]
    private AudioClip rip = null;

    [SerializeField]
    private AudioSource ripSource;
    
    protected void Awake()
    {
        basePositionLogo = logo.position.y;
        gameModePanel.localScale = Vector3.zero;
        gameModeOverlay.DOFade(0, 0);
        gameModePopup.SetActive(false);
    }

    public void OnPlay()
    {
        gameModePopup.SetActive(true);
        gameModeOverlay.DOFade(1, 0.5f);
        gameModePanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);
    }

    public void SelectGameMode(int gameMode)
    {
        PlayerPrefs.SetInt("GameMode", gameMode);
        SceneManager.LoadScene("Game");
    }

    public void OnQuit()
    {
        Application.Quit(0);
    }

    public void OnPressedAny()
    {
        
        logo.position = new Vector2(logo.position.x, 1200);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        logo.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        pressKey.gameObject.SetActive(false);

        Sequence seq = DOTween.Sequence();


        seq.Insert(2, background.DOFade(1, 5)).SetEase(Ease.OutElastic);
        seq.Insert(2, logo.DOMoveY(basePositionLogo, 6)).SetEase(Ease.OutExpo);
        seq.Insert(3, buttonsAndStuff.DOFade(1, 8)).SetEase(Ease.OutExpo);

        ripSource.clip = (rip);
        ripSource.loop = false;
        ripSource.Play();
        seq.SetLink(this.gameObject);

    }
}
