using System;
using Cyberultimate.Unity;
using UnityEngine;

namespace Player
{
    public class PlayerAnim : MonoSingleton<PlayerAnim>
    {
        private Animator animator;
        
        private static readonly int ShootTrigger = Animator.StringToHash("Shoot");
        private static readonly int PunchTrigger = Animator.StringToHash("Punch");
        private static readonly int Hit = Animator.StringToHash("ItemHit");

        public void SetIsWalking(bool v) /*=> animator.SetBool(IsWalking, v);*/
        {
        }

        public void Punch() => animator.SetTrigger(PunchTrigger);
        public void Shoot() => animator.SetTrigger(ShootTrigger);
        public void ItemHit() => animator.SetTrigger(Hit);
        
        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            base.Awake();
        }
    }
}
