using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using LetterBattle.Utility;
using UnityEngine;

public class PauseManager : MonoSingleton<PauseManager>
{
    [SerializeField]
    private GameObject pauseObject = null;
    [SerializeField]
    private GameObject gameOverObject = null;

    private bool isPaused = false;

    public void SwitchPause()
    {
        isPaused = !isPaused;
        Pause(pauseObject, isPaused);
    }

    public void SwitchDeath()
    {
        isPaused = !isPaused;
        Pause(gameOverObject, isPaused);
    }

    public void Pause(GameObject obj, bool isTrue)
    {
        TimeScaling.Status.Register(obj, isTrue ? 0 : 1);
        obj.SetActive(isTrue);
    }
}
