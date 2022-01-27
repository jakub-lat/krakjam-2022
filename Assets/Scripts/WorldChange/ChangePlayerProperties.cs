using StarterAssets;
using System;
using UnityEngine;
using WorldChange;

[Serializable]
public class PlayerPropertiesData
{
    public float moveSpeed;
}

public class ChangePlayerProperties : WorldChangeLogic
{
    [SerializeField]
    private WorldTypeDict<PlayerPropertiesData> data;

    private FirstPersonController fpsc;

    private void Start()
    {
        fpsc = GetComponent<FirstPersonController>();
    }

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        var properties = data[type];
        fpsc.MoveSpeed = properties.moveSpeed;
    }
}
