using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KrakJam2022.Player;
using Player;
using UnityEngine;

namespace WorldChange
{
    public class ChangeMaterial : WorldChangeLogic
    {
        private readonly float duration = 0.5f;

        [SerializeField] private WorldTypeDict<Material> materials;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            DOTween.SetTweensCapacity(6250, 100);
        }
        

        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            var from = materials.GetInverse(type);
            var to = materials[type];

            var temp = new Material(from);
            meshRenderer.material = temp;
            
            DOTween.To(() => 0f, (v) => meshRenderer.material.Lerp(from, to, v), 1, duration)
                .SetEase(Ease.InOutQuint)
                .OnComplete(
                    () => { meshRenderer.material = to; }).SetLink(this.gameObject);

            // StartCoroutine(MaterialAfterDelay(to));
        }

        // private IEnumerator MaterialAfterDelay(Material to)
        // {
        //     yield return new WaitForSeconds(duration / 2);
        //     meshRenderer.material = to;
        // }
    }
}
