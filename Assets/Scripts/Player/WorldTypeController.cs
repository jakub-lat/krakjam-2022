using Cyberultimate.Unity;
using UnityEngine;
using WorldChange;

namespace Player
{
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
                x.OnWorldTypeChange(type);
            }
        }

        public void OnWorldChange()
        {
            SetWorldType(currentWorldType == WorldType.Normal ? WorldType.Psycho : WorldType.Normal);
        }
    }
}
