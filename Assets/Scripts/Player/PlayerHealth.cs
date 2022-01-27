using Cyberultimate.Unity;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoSingleton<PlayerHealth>
    {
        [SerializeField] private float health;
        public float Health
        {
            get => health;
            set
            {
                if (value <= 0) Die();
                else health = value;
            }
        }

        private void Die()
        {
            if (PauseManager.Current.IsDead)
            {
                return;
            }

            PauseManager.Current.SwitchDeath();
        }

        // todo - cool UI
    }
}
