using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
    [SerializeField]
    private string poolTag = "PopupText";

    [SerializeField]
    private float offset;

    public void SpawnStandardDamage(Enemy enemy, int dmg)
    {
        GetFundaments(enemy, dmg);
    }

    private TextMesh GetFundaments(Enemy enemy, int dmg)
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(poolTag, enemy.transform.position + new Vector3(0, 1.5f, 0), enemy.transform.rotation);
        TextMesh text = obj.GetComponent<TextMesh>();
        text.text = dmg.ToString();
        return text;
    }

    public void SpawnHeadshotDamage(Enemy enemy, int dmg)
    {
        TextMesh text = GetFundaments(enemy, dmg);
        text.color = Color.yellow;
    }
}
