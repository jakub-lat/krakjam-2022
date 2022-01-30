using System;
using Cyberultimate.Unity;
using Game;
using Scoreboard;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoSingleton<PlayerHealth>
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField]
        private AudioSource soundSource;

        [SerializeField]
        private FootstepSoundController soundController;
        public float MaxHealth => maxHealth;
        public float Health
        {
            get => health;
            set
            {
                if (value < health)
                {
                    soundSource.PlayOneShot(soundController.GetRandomSoundFromRange());
                }

                if (value > health)
                {
                    DamageSpriteChanger.Current?.Blink();
                }

                health = Math.Min(value, maxHealth);
                PercentageOverlay.Get(OverlayType.Health).UpdateAmount(1 - (health / maxHealth));
                DamageSpriteChanger.Current?.SetHpPercent(health / maxHealth);

                if (health <= 0) Die();
            }
        }

        private void Start()
        {
            maxHealth = MaxHealth - (5 * (int)LevelManager.Current.GameMode);
            health = maxHealth;
        }

        private void Die()
        {
            if (PauseManager.Current.IsDead)
            {
                return;
            }

            GameScoreboard.Current.levelData.deaths++;

            PauseManager.Current.SwitchDeath();
        }
    }
}
