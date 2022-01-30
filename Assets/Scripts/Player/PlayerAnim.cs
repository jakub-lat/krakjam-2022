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

        public void SetIsWalking(bool v) /*=> animator.SetBool(IsWalking, v);*/
        {
        }

        public void Punch() => animator.SetTrigger(PunchTrigger);
        public void Shoot() => animator.SetTrigger(ShootTrigger);
        
        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            base.Awake();
        }
    }
}
