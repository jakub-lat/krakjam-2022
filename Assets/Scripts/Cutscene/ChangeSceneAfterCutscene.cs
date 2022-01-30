using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterCutscene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private GameObject skipIndictator;

    public void OnPause()
    {
        director.Stop();
    }

    public void OnInteract()
    {
        director.Stop();
    }

    public void OnAny()
    {
        skipIndictator.SetActive(true);
    }

    protected void Awake()
    {
        director.stopped += Director_stopped;
    }

    protected void OnDestroy()
    {
        director.stopped -= Director_stopped;
    }

    private void Director_stopped(PlayableDirector obj)
    {
        SceneManager.LoadScene(sceneName);
    }
}
