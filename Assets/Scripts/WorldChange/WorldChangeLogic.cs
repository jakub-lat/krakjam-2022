
using System;
using UnityEngine;


public abstract class WorldChangeLogic : MonoBehaviour
{
    protected virtual void Start()
    {
        OnWorldTypeChange(WorldTypeController.Current.CurrentWorldType);
    }

    public abstract void OnWorldTypeChange(WorldTypeController.WorldType type);
}
