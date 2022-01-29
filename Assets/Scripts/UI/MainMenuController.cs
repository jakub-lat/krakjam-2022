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
    private Transform pulseLeft;

    [SerializeField]
    private Transform pulseRight;

    [SerializeField]
    private Image background;

    [SerializeField]
    private GameObject buttonsAndStuff = null;

    [SerializeField] private GameObject gameModePopup;
    [SerializeField] private RectTransform gameModePanel;
    [SerializeField] private Image gameModeOverlay;
    
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

    // logo: 637 -> 0, szybki wślizg
    // pulse left i pulse right na środku, odsunięcie po lewej i prawej
    public void OnPressedAny()
    {
        logo.position = new Vector2(logo.position.x, 637);
        pulseLeft.position = new Vector2(0, 0);
        pulseRight.position = new Vector2(0, 0);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        logo.gameObject.SetActive(true);
        pulseLeft.gameObject.SetActive(true);
        pulseRight.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        buttonsAndStuff.gameObject.SetActive(true);
        pressKey.gameObject.SetActive(false);

        Sequence seq = DOTween.Sequence();


        seq.Insert(0, pulseLeft.DOMoveX(-700, 1)).SetEase(Ease.OutElastic);
        seq.Insert(1, pulseRight.DOMoveX(737, 1)).SetEase(Ease.OutElastic);
        seq.Insert(2, background.DOFade(1, 3)).SetEase(Ease.OutElastic);
        seq.Insert(2, logo.DOMoveY(basePositionLogo, 2)).SetEase(Ease.OutExpo);


        seq.SetLink(this.gameObject);

    }
}
