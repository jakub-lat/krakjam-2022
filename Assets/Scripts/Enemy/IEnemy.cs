using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public interface IEnemy
    {
        public void GotHit(float damage);
        public void PlaySound(AudioClip clip);
    }
}
