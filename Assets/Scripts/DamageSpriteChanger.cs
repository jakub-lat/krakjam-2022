using System;
using Player;
using System.Collections;
using System.Collections.Generic;
using Cyberultimate.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpriteChanger : MonoSingleton<DamageSpriteChanger>
{
    [SerializeField]
    private Sprite[] dmgSprites = null;

    [SerializeField]
    private Image portraitImg = null;

    [SerializeField] private Image backgroundImg;

    [SerializeField] private Gradient bgGradient;

    private int GetIndex(float hpPercent, int count)
    {
        return (int)Math.Round(hpPercent * (count - 1));
    }
    
    public void SetHpPercent(float hpPercent)
    {
        portraitImg.sprite = dmgSprites[GetIndex(hpPercent, dmgSprites.Length)];
        backgroundImg.DOColor(bgGradient.Evaluate(hpPercent), 0.5f).SetEase(Ease.OutQuint);
    }
}
