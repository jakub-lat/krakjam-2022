
using UnityEngine;


public abstract class WorldChangeLogic : MonoBehaviour
{
    public abstract void OnWorldTypeChange(WorldTypeController.WorldType type);
}
