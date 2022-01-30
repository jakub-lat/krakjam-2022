using System;
using Cyberultimate.Unity;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class GameMusic : WorldChangeLogic
    {
        public WorldTypeDict<AudioSource> musicSources;
        private AudioSource CurrentSource => musicSources[WorldTypeController.Current.CurrentWorldType];

        public float fadeDuration = 0.5f;
        
        public static GameMusic Current;
        
        private void Awake()
        {
            Current = this;
        }


        public void FadeIn(float dur = 1f)
        {
            musicSources[WorldTypeController.WorldType.Normal].Play();
            musicSources[WorldTypeController.WorldType.Psycho].Play();
            CurrentSource.DOFade(1, dur);
        }

        public void FadeOut(float dur = 1f)
        {
            CurrentSource.DOFade(0, dur).OnComplete(() =>
            {
                musicSources[WorldTypeController.WorldType.Normal].Pause();
                musicSources[WorldTypeController.WorldType.Psycho].Pause();
            });
        }
        
        public override void OnWorldTypeChange(WorldTypeController.WorldType type)
        {
            var from = musicSources.GetInverse(type);
            var to = musicSources[type];

            from.DOFade(0, fadeDuration);
            to.DOFade(1, fadeDuration);
        }
    }
}
