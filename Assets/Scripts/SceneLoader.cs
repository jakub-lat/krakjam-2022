using System.Collections;
using Cyberultimate.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoSingleton<SceneLoader>
{
    private CanvasGroup cg;
    [SerializeField] private Image progressBar;

    protected override void Awake()
    {
        base.Awake();
        if (Current == this)
        {
            DontDestroyOnLoad(gameObject);
        }

        cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.alpha = 0;
    }

    public static void LoadScene(string sceneName)
    {
        if (Current != null)
        {
            Current.StartCoroutine(Current.LoadSceneEnumerator(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    public IEnumerator LoadSceneEnumerator(string sceneName)
    {
        cg.blocksRaycasts = true;
        progressBar.fillAmount = 0;
        yield return cg.DOFade(1, 0.2f).SetEase(Ease.InOutCubic).WaitForCompletion();
        var task = SceneManager.LoadSceneAsync(sceneName);
        
        while (!task.isDone)
        {
            progressBar.fillAmount = task.progress;
            yield return null;
        }
        cg.DOFade(0, 0.2f).SetEase(Ease.InOutCubic);
        cg.blocksRaycasts = false;
    }
}
