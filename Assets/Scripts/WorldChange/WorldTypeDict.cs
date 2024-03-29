﻿using System;
using System.Collections.Generic;

[Serializable]
public class WorldTypeDict<T>
{
    public T normal;
    public T psycho;

    public T this[WorldTypeController.WorldType index]
    {
        get => index == WorldTypeController.WorldType.Normal ? normal : psycho;
    }

    public Dictionary<WorldTypeController.WorldType, T> Dict => new()
    {
        { WorldTypeController.WorldType.Normal, normal },
        { WorldTypeController.WorldType.Psycho, psycho }
    };

    public T GetInverse(WorldTypeController.WorldType index)
    {
        return index == WorldTypeController.WorldType.Normal ? psycho : normal;
    }
}
