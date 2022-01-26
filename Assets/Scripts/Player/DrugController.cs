using System;
using Cyberultimate.Unity;
using Player;
using UI;
using UnityEngine;
using WorldChange;

namespace KrakJam2022.Player
{
    public class DrugController : MonoSingleton<DrugController>
    {
        [SerializeField] private int initialDoseCount;
        [SerializeField] private float drugEffectTime;
        [SerializeField] private float addictionMaxTime;
        [SerializeField] private float addictionEffectsStartTime;

        private bool IsOnDrugs => WorldTypeController.Current.CurrentWorldType == WorldTypeController.WorldType.Psycho;
        
        private int currentDoses = 0;
        private float timer = 0;

        private void Start()
        {
            currentDoses = initialDoseCount;
        }

        public void AddDrugs(int amount)
        {
            currentDoses += amount;
        }

        public void Use()
        {
            if (IsOnDrugs || currentDoses <= 0) return;
            
            currentDoses--;
            timer = 0;
            // todo animation?
            WorldTypeController.Current.SetWorldType(WorldTypeController.WorldType.Psycho);
        }

        public void End()
        {
            // todo animation
            timer = 0;
            WorldTypeController.Current.SetWorldType(WorldTypeController.WorldType.Normal);
        }

        private void Update()
        {
            // todo nie robić tego w update
            if(DrugUI.Current) DrugUI.Current.SetInfo($"{currentDoses} | is on drugs: {IsOnDrugs}");
            
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
