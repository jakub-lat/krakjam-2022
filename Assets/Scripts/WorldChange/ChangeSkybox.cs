using System;
using DG.Tweening;
using UnityEngine;

namespace WorldChange
{
    public class ChangeSkybox : WorldChangeLogic
    {
        [SerializeField] private WorldTypeDict<Material> skyboxes;
        [SerializeField] private float duration;

        private void Start()
        {
            // RenderSettings.skybox = skyboxes[WorldTypeController.WorldType.Normal];
        }

        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            Debug.Log($"world changing to {type}");
            
            var from = skyboxes.GetInverse(type);
            var to = skyboxes[type];

            var temp = new Material(from);

            RenderSettings.skybox = temp;
            
            DOTween.To(() => 0f, (v) => RenderSettings.skybox.Lerp(from, to, v), 1, duration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(
                    () => { RenderSettings.skybox = to; });
        }

        private void OnApplicationQuit()
        {
            RenderSettings.skybox = skyboxes[WorldTypeController.WorldType.Normal];
        }
    }
}
