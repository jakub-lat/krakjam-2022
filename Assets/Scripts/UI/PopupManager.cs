using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupManager : MonoSingleton<PopupManager>
{
    [SerializeField]
    private string poolTag = "PopupText";

    [SerializeField]
    private float duration;

    [SerializeField]
    private Ease ease = Ease.OutElastic;

    [SerializeField]
    private Vector2 minMaxScale;

    /// <summary>
    /// X position
    /// </summary>
    [SerializeField]
    private Vector2 minMaxPosition;

    public void SpawnStandardDamage(Transform enemy, int dmg)
    {
        float scale = Random.Range(minMaxScale.x, minMaxScale.y);
        float positionX = Random.Range(minMaxPosition.x, minMaxPosition.y);

        GetFundaments(enemy, dmg, new Vector3(scale, scale, scale), positionX);
    }

    private TextMesh GetFundaments(Transform enemy, int dmg, Vector3 maxScale, float positionX)
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(poolTag, enemy.position + new Vector3(positionX, 0, 0), enemy.rotation);
        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x,obj.transform.localEulerAngles.y+180,obj.transform.localEulerAngles.z);
        
        TextMesh text = obj.GetComponent<TextMesh>();
        text.transform.localScale = new Vector3(0, 0, 0);
        text.transform.DOScale(new Vector3(1, 1, 1), duration / 2).SetEase(ease).SetLink(this.gameObject)
            .OnComplete(() => text.transform.DOScale(new Vector3(0, 0, 0), duration * 1.5f).SetEase(ease).SetLink(this.gameObject));

        text.transform.DOMoveY(text.transform.position.y + 1, duration).SetEase(ease).SetLink(this.gameObject);
        text.text = dmg.ToString();
        return text;
    }

    public void SpawnHeadshotDamage(Transform enemy, int dmg)
    {
        float scale = Random.Range(minMaxScale.x, minMaxScale.y);
        float positionX = Random.Range(minMaxPosition.x, minMaxPosition.y);

        TextMesh text = GetFundaments(enemy, dmg, new Vector3(scale * 1.5f, scale * 1.5f, scale * 1.5f), positionX);
        text.color = Color.yellow;
    }
}
