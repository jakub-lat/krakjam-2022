using KrakJam2022.Player;
using UnityEngine;

namespace WorldChange
{
    public abstract class WorldChangeLogic : MonoBehaviour
    {
        public abstract void OnWorldTypeChange(WorldTypeController.WorldType type);
    }
}
