using UnityEngine;
using WorldChange;

namespace KrakJam2022.Player
{
    public class WorldTypeController : MonoBehaviour
    {
        public enum WorldType
        {
            Normal, Psycho
        }

        private WorldType currentWorldType;
        public WorldType CurrentWorldType => currentWorldType;

        public void ChangeWorldType(WorldType type)
        {
            currentWorldType = type;

            var objs = GameObject.FindObjectsOfType<WorldChangeLogic>();
            foreach (var x in objs)
            {
                x.OnWorldTypeChange(type);
            }
        }

        public void OnWorldChange()
        {
            ChangeWorldType(currentWorldType == WorldType.Normal ? WorldType.Psycho : WorldType.Normal);
        }
    }
}
