using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cyberultimate.Unity;
using Cyberultimate;
using LetterBattle.Utility;

public class HitmarkManager : MonoSingleton<HitmarkManager>
{
    [SerializeField]
    private Image[] hitmarkElements = null;

    [SerializeField]
    private GameObject hitmarkObj = null;

    [SerializeField]
    private float timeToDisappear = 2;

    private Cooldown c;

    protected override void Awake()
    {
        base.Awake();
    }

    public void GetNormalHit()
    {
        ChangeColor(Color.white);
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        hitmarkObj.SetActive(true);
        yield return new WaitForSeconds(timeToDisappear);
        hitmarkObj.SetActive(false);
    }

    private void ChangeColor(Color color)
    {
        if (hitmarkElements[0].color != color)
        {
            for (int i = 0; i < hitmarkElements.Length; i++)
            {
                Image elem = hitmarkElements[i];
                elem.color = color;
            }
        }
    }

    public void GetHeadshotHit()
    {
        ChangeColor(Color.red);
        StartCoroutine(Hit());
    }
}
