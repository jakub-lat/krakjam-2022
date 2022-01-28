using System;
using Cinemachine;
using Cyberultimate.Unity;
using UnityEngine;

namespace Player
{
    public class CinemachineVCamInstance : MonoSingleton<CinemachineVCamInstance>
    {
        public CinemachineVirtualCamera Cam { get; private set; }

        private void Start()
        {
            Cam = GetComponent<CinemachineVirtualCamera>();
        }
    }
}
