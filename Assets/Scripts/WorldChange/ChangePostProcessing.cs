using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangePostProcessing : WorldChangeLogic
{
    [SerializeField] private WorldTypeDict<Volume> volumes;
    [SerializeField] private float transitionDuration;

    public override void OnWorldTypeChange(WorldTypeController.WorldType type)
    {
        var from = volumes.GetInverse(type);
        var to = volumes[type];

        DOTween.To(() => to.weight, (v) => to.weight = v, 1, transitionDuration).SetEase(Ease.InOutQuint);
        DOTween.To(() => from.weight, (v) => from.weight = v, 0, transitionDuration).SetEase(Ease.InOutQuint);
    }
}
