using System;
using Cyberultimate.Unity;
using UnityEngine;

public class WorldTypeController : MonoSingleton<WorldTypeController>
{
    public enum WorldType
    {
        Normal, Psycho
    }

    private WorldType currentWorldType;
    public WorldType CurrentWorldType => currentWorldType;

    private void Start()
    {
        SetWorldType(currentWorldType);
    }

    public void SetWorldType(WorldType type)
    {
        currentWorldType = type;

        var objs = GameObject.FindObjectsOfType<WorldChangeLogic>();
        foreach (var x in objs)
        {
            try
            {
                x.OnWorldTypeChange(type);
            }
            catch (Exception e)
            {
                Debug.LogError(x.gameObject.name + " " + e);
            }
        }
    }

    public void OnWorldChange()
    {
        SetWorldType(currentWorldType == WorldType.Normal ? WorldType.Psycho : WorldType.Normal);
    }
}
