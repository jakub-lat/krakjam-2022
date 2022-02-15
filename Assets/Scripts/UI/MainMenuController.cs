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
    private GameObject pressKey = null;
    private float basePositionLogo;

    // [SerializeField]
    // private Image background;

    [SerializeField]
    private CanvasGroup buttonsAndStuff = null;

    [SerializeField] private GameObject gameModePopup;
    [SerializeField] private RectTransform gameModePanel;
    [SerializeField] private Image gameModeOverlay;

    [SerializeField]
    private AudioClip rip = null;

    [SerializeField]
    private AudioSource ripSource;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private CanvasGroup options = null;

    [SerializeField] private Image optionsOverlay;

    /*
    [SerializeField]
    private Button optionsBtn = null;
    private Text optionsTxt = null;
    */

    private bool doOnce;

    [SerializeField]
    private float deadScale = 20;

    [SerializeField]
    private float deadScaleDuration = 4;

    protected void Awake()
    {
        // basePositionLogo = logo.position.y;
        gameModePanel.localScale = Vector3.zero;
        gameModeOverlay.DOFade(0, 0);
        gameModePopup.SetActive(false);
        optionsOverlay.DOFade(0, 0);
        OnOutOptions();
    }

    protected void Start()
    {
        // optionsTxt = optionsBtn.transform.GetChild(0).GetComponent<Text>();
        logo.localScale = Vector3.zero;
        buttonsAndStuff.alpha = 0;
        buttonsAndStuff.blocksRaycasts = false;
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
        SceneLoader.LoadScene("Intro");
    }

    public void OnQuit()
    {
        Application.Quit(0);
    }

    public void OnOutOptions()
    {
        optionsOverlay.DOFade(0, 0.5f);
        options.DOFade(0, 0.3f);
        options.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            options.gameObject.SetActive(false);
        });
        // optionsBtn.onClick.RemoveAllListeners();
        // optionsBtn.onClick.AddListener(OnOptions);
        // optionsTxt.text = "Options";
    }

    public void OnOptions()
    {
        optionsOverlay.DOFade(1, 0.5f);
        options.gameObject.SetActive(true);
        options.DOFade(1, 0.5f);
        options.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuint);

        // optionsBtn.onClick.RemoveAllListeners();
        // optionsBtn.onClick.AddListener(OnOutOptions);
        // optionsTxt.text = "Back";
    }

    public void OnPressedAny()
    {
        if (doOnce)
        {
            return;
        }

        // logo.position = new Vector2(logo.position.x, 1200);
        // background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        // logo.gameObject.SetActive(true);
        // background.gameObject.SetActive(true);
        pressKey.SetActive(false);

        Sequence seq = DOTween.Sequence();
        // seq.Insert(5.5f, background.DOFade(1, 5)).SetEase(Ease.OutElastic);
        seq.Insert(7, DOVirtual.Float(1, deadScale, deadScaleDuration, (x) => lineRender.widthMultiplier = x)).SetEase(Ease.OutExpo);
        seq.Insert(7, logo.DOScale(new Vector2(1, 1), 7)).SetEase(Ease.OutExpo);
        seq.Insert(7, buttonsAndStuff.DOFade(1, 4)).SetEase(Ease.OutExpo);

        seq.InsertCallback(7, () =>
        {
            buttonsAndStuff.blocksRaycasts = true;
        });
        
        ripSource.clip = (rip);
        ripSource.loop = false;
        ripSource.Play();

        seq.Insert(5.5f, musicSource.DOFade(0.1f, 7).SetEase(Ease.OutElastic));

        seq.SetLink(this.gameObject);
        doOnce = true;

    }
}
