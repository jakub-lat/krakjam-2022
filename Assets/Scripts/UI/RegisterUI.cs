using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Scoreboard;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    [SerializeField]
    private Transform window = null;

    [SerializeField]
    private float scaleDuration = 1.5f;

    [SerializeField]
    private Ease scaleEase = Ease.OutElastic;

    [SerializeField]
    private Button cancelButton = null;

    [SerializeField] private Button submitBtn;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Text loadingText;
    [SerializeField] private Text errorText;

    protected void Start()
    {
        // PlayerPrefs.DeleteKey(GameScoreboard.TokenKey);
        
        window.localScale = new Vector3(0, 0, 0);
        loadingText.enabled = false;
        errorText.enabled = false;
        
        TryLogin();
    }

    private async void TryLogin()
    {
        if (GameScoreboard.Current.LoggedIn)
        {
            try
            {
                await GameScoreboard.Current.GetCurrentPlayer();
                submitBtn.enabled = false;
                LoadScene();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }

    public void SetupContactForm()
    {
        cancelButton.gameObject.SetActive(true);
        window.DOScale(new Vector3(1, 1, 1), scaleDuration).SetEase(scaleEase).SetLink(this.gameObject);
    }

    public void SetdownContactForm()
    {
        window.DOScale(new Vector3(0, 0, 0), scaleDuration).SetEase(scaleEase).SetLink(this.gameObject);
    }

    public void OnSubmit()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            errorText.enabled = true;
            errorText.text = "Please enter your name correctly.";
            return;
        }
        
        submitBtn.enabled = false;
        errorText.enabled = false;
        loadingText.enabled = true;
        
        PlayerPrefs.DeleteKey(GameScoreboard.TokenKey);

        try
        {
            GameScoreboard.Current.Register(nameInput.text);
        }
        catch
        {
            // errorText.text = $"Something went wrong!";
            errorText.enabled = true;
            Invoke(nameof(LoadScene), 2f);
            return;
        }

        loadingText.enabled = false;
        LoadScene();
    }

    private void LoadScene()
    {
        SceneLoader.LoadScene("MainMenu");
    }

    public void Cancel()
    {
        SceneLoader.LoadScene("MainMenu");
    }

}
