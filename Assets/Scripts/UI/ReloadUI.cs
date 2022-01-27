using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoSingleton<ReloadUI>
{
    [SerializeField]
    private Image reloadCircle = null;

    public void SetInfo(float timer, float duration)
    {
        reloadCircle.fillAmount = (-timer / duration) + 1;
        if (reloadCircle.fillAmount >= 1)
        {
            reloadCircle.fillAmount = 0;
        }
    }
}
