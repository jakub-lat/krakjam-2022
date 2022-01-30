using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using KrakJam2022.Player;
using Player;
using UnityEngine;

namespace WorldChange
{
    public class ChangeMaterialSkinnedMR : WorldChangeLogic
    {
        private readonly float duration = 0.5f;

        [SerializeField] private WorldTypeDict<Material> materials;

        [SerializeField] private WorldTypeDict<Material[]> multipleMaterials;

        public SkinnedMeshRenderer meshRenderer;

        private void Awake()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<SkinnedMeshRenderer>();
            }

            if (multipleMaterials.psycho is { Length: > 1 })
            {
                // Debug.Log(gameObject.name + " WOOOW COUNT>1, COUNT=" + meshRenderer.materials.Length);
                multipleMaterials.normal = meshRenderer.materials;
            }

            DOTween.SetTweensCapacity(6250, 100);
        }


        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            if (multipleMaterials.psycho is { Length: > 1 })
            {
                var froms = multipleMaterials.GetInverse(type);
                var tos = multipleMaterials[type];

                var arr = new Material[froms.Length];
                for (var i = 0; i < froms.Length; i++)
                {
                    var from = froms[i];
                    var to = tos[i];
                    var temp = new Material(from);
                    arr[i] = temp;
                }
                meshRenderer.materials = arr;

                DOTween.To(() => 0f, (v) =>
                    {
                        var arr = new Material[froms.Length];
                        for (var i = 0; i < froms.Length; i++)
                        {
                            var from = froms[i];
                            var to = tos[i];
                            var temp = meshRenderer.materials[i];
                            temp.Lerp(from, to, v);
                            arr[i] = temp;
                        }

                        meshRenderer.materials = arr;

                    }, 1, duration)
                    .SetEase(Ease.InOutQuint)
                    .OnComplete(
                        () => { meshRenderer.materials = tos; }).SetLink(this.gameObject);
            }
            else
            {
                var from = materials.GetInverse(type);
                var to = materials[type];
            
                var temp = new Material(from);
                meshRenderer.material = temp;
            
                DOTween.To(() => 0f, (v) => meshRenderer.material.Lerp(from, to, v), 1, duration)
                    .SetEase(Ease.InOutQuint)
                    .OnComplete(
                        () => { meshRenderer.material = to; }).SetLink(this.gameObject);
            }

            // StartCoroutine(MaterialAfterDelay(to));
        }

        // private IEnumerator MaterialAfterDelay(Material to)
        // {
        //     yield return new WaitForSeconds(duration / 2);
        //     meshRenderer.material = to;
        // }
    }
}
