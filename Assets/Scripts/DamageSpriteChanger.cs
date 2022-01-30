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

    [SerializeField] private Image blink;

    private int GetIndex(float hpPercent, int count)
    {
        return (int)Math.Round(hpPercent * (count - 1));
    }
    
    public void SetHpPercent(float hpPercent)
    {
        portraitImg.sprite = dmgSprites[GetIndex(hpPercent, dmgSprites.Length)];
        backgroundImg.DOColor(bgGradient.Evaluate(hpPercent), 0.5f).SetEase(Ease.OutQuint);
    }

    public void Blink()
    {
        blink.DOFade(0.5f, 0.3f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            blink.DOFade(0, 0.5f).SetDelay(1f).SetEase(Ease.InCubic);
        });
    }
}
