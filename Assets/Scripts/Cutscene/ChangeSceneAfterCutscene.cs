using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterCutscene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void OnCutsceneEnd()
    {
        SceneManager.LoadScene(sceneName);
    }
}
