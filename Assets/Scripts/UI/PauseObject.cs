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
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRevive()
    {
        SceneManager.LoadScene("Jakub");
    }
}
