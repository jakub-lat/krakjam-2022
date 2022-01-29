using System;
using Cinemachine;
using Cyberultimate.Unity;
using UnityEngine;

namespace Player
{
    public class CinemachineVCamInstance : MonoSingleton<CinemachineVCamInstance>
    {
        public CinemachineVirtualCamera Cam { get; private set; }
        private CinemachinePOV pov;

        protected override void Awake()
        {
            base.Awake();

            Cam = GetComponent<CinemachineVirtualCamera>();
            pov = Cam.GetCinemachineComponent<CinemachinePOV>();
            OptionsMenu.SensitivityMouse = pov.m_HorizontalAxis.m_MaxSpeed;
        }

        protected void OnDestroy()
        {
        }

        protected void Update()
        {
            pov.m_VerticalAxis.m_MaxSpeed = OptionsMenu.SensitivityMouse;
            pov.m_HorizontalAxis.m_MaxSpeed = OptionsMenu.SensitivityMouse;
        }
    }
}
