using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
    [SerializeField]
    private string poolTag = "PopupText";

    public void SpawnStandardDamage(Enemy enemy)
    {
        GameObject obj = ObjectPooler.Current.SpawnPool(poolTag, new Vector3(0, 1, 0), Quaternion.identity);
        obj.transform.position = new Vector3(0, 1, 0);
        obj.transform.localPosition = new Vector3(0, 1, 0);
        
        obj.transform.SetParent(enemy.PopupCanvas.transform);
    }
}
