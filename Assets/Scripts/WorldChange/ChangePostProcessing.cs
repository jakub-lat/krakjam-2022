using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangePostProcessing : WorldChangeLogic
{
    [SerializeField] private WorldTypeDict<Volume> volumes;
    [SerializeField] private float transitionDuration;

    private Volume currentVolume;

    public static ChangePostProcessing Current;

    private void Awake()
    {
        Current = this;
    }

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        var from = volumes.GetInverse(type);
        var to = volumes[type];

        currentVolume = to;

        DOTween.To(() => to.weight, (v) => to.weight = v, 1, transitionDuration).SetEase(Ease.InOutQuint);
        DOTween.To(() => from.weight, (v) => from.weight = v, 0, transitionDuration)
            .SetEase(Ease.InOutQuint);
        
        if (type == WorldTypeController.WorldType.Psycho)
        {
            var l = (LensDistortion)currentVolume.profile.components.FirstOrDefault(x => x is LensDistortion);
            if (l != null)
            {
                var originalIntensity = l.intensity.value;
                DOTween.To(() => l.intensity.value, (v) => l.intensity.value = v, originalIntensity * 1.5f, transitionDuration).SetEase(Ease.InOutElastic)
                    .OnComplete(
                    () =>
                    {
                        DOTween.To(() => l.intensity.value, (v) => l.intensity.value = v, originalIntensity,
                            transitionDuration).SetEase(Ease.InOutElastic);
                    });
            }
        }
    }
}
