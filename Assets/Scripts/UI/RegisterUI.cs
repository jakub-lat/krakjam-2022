using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    [SerializeField] private InputField nameInput;

    protected void Start()
    {
        window.localScale = new Vector3(0, 0, 0);
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
        // setup scoreboard here
        SceneManager.LoadScene("MainMenu");
    }

    public void Cancel()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
