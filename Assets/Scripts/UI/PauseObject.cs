using LetterBattle.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseObject : MonoBehaviour
{
    public void OnResume()
    {
        PauseManager.Current.SwitchPause();
    }

    public void OnQuit()
    {
        TimeScaling.Status.Unregister(PauseManager.Current.LastObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRevive()
    {
        TimeScaling.Status.Unregister(PauseManager.Current.LastObject);
        SceneManager.LoadScene("Game");
    }
}
