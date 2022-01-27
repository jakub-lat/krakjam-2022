using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using LetterBattle.Utility;
using UnityEngine;

public class PauseManager : MonoSingleton<PauseManager>
{
    [SerializeField]
    private GameObject pauseObject = null;

    private bool isPaused = false;

    public void SwitchPause()
    {
        isPaused = !isPaused;
        Pause(isPaused);
    }

    public void Pause(bool isTrue)
    {
        TimeScaling.Status.Register(this, isTrue ? 0 : 1);
        pauseObject.SetActive(isTrue);
    }
}
