using System.Collections.Generic;
using KrakJam2022.Player;
using Player;
using UnityEngine;

namespace WorldChange
{
    public class ChangeModel : WorldChangeLogic
    {
        [SerializeField] private WorldTypeDict<GameObject> models;
        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            foreach (var (k, v) in models.Dict)
            {
                v.SetActive(k == type);
            }
        }
    }
}
