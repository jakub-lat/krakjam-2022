using Cyberultimate.Unity;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoSingleton<PlayerHealth>
    {
        private float health;
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
            // todo die
            Debug.Log("YOU DIED");
        }

        // todo - cool UI
    }
}
