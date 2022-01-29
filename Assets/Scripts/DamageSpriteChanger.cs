using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSpriteChanger : MonoBehaviour
{
    [SerializeField]
    private Sprite[] dmgSprites = null;

    [SerializeField]
    private Image portraitImg = null;

    private float[] GetNumbers()
    {

        // 0 -> healed
        return new float[]
        {
            0.2f,
            0.4f,
            0.6f,
            0.8f
        };
    }

    protected void Update()
    {
        float[] numbersMasonWhatDoTheyMean = GetNumbers();
        for (int i = 0; i < numbersMasonWhatDoTheyMean.Length; i++)
        {
            if (numbersMasonWhatDoTheyMean[i] >= PlayerHealth.Current.Health / PlayerHealth.Current.MaxHealth)
            {
                portraitImg.sprite = dmgSprites[i];
                return;
            }
        }

        portraitImg.sprite = dmgSprites[dmgSprites.Length - 1];


    }
}
