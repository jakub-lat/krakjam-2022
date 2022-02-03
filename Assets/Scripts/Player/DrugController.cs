using System;
using Cyberultimate.Unity;
using Game;
using Player;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using WorldChange;

namespace KrakJam2022.Player
{
    public class DrugController : MonoSingleton<DrugController>
    {
        [SerializeField] private int initialDoseCount;
        [SerializeField] private float baseDrugEffectTime;
        [SerializeField] private float subtractDrugEffectTimeBasedOnGameModeMultiplier;
        private float drugEffectTime;
        [SerializeField] private float addictionMaxTime;
        [SerializeField] private float addictionEffectsStartTime;
        [SerializeField] private float drugHealthBoost;
        [SerializeField] private float subtractDrugHealthBoostBasedOnGameMode = 20;

        private bool IsOnDrugs => WorldTypeController.Current.CurrentWorldType == WorldTypeController.WorldType.Psycho;
        
        private int currentDoses = 0;
        private float timer = 0;

        [SerializeField]
        private AudioSource soundSource;

        [SerializeField]
        private AudioClip takeDrug;

        [SerializeField]
        private AudioClip sensation;

        private void Start()
        {
            currentDoses = initialDoseCount;
            drugEffectTime = baseDrugEffectTime - ((int)LevelManager.Current.GameMode * subtractDrugEffectTimeBasedOnGameModeMultiplier);
        }

        public void AddDrugs(int amount)
        {
            currentDoses += amount;
        }

        private void PlaySensation()
        {
            soundSource.PlayOneShot(sensation);
        }

        public void Use()
        {
            if (IsOnDrugs || currentDoses <= 0) return;


            PlayerHealth.Current.Health += drugHealthBoost - ((int)LevelManager.Current.GameMode * subtractDrugHealthBoostBasedOnGameMode);

            soundSource.PlayOneShot(takeDrug);
            Invoke(nameof(PlaySensation), 1.5f);
            currentDoses--;
            timer = 0;
            WorldTypeController.Current.SetWorldType(WorldTypeController.WorldType.Psycho);
        }

        public void End()
        {
            timer = 0;
            WorldTypeController.Current.SetWorldType(WorldTypeController.WorldType.Normal);
        }

        private void Update()
        {
            // todo nie robić tego w update
            if(DrugUI.Current) DrugUI.Current.SetInfo(currentDoses);
            
            timer += Time.deltaTime;
            if (IsOnDrugs)
            {
                if (timer >= drugEffectTime)
                {
                    End();
                }
            }
            else
            {
                if (timer >= addictionEffectsStartTime)
                {
                    var sideEffectsPercent = (timer - addictionEffectsStartTime) / addictionMaxTime;
                }

                if (timer >= addictionMaxTime)
                {
                    // todo die
                }
            }
        }
    }
}
