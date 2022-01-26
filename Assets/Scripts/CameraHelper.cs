using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoSingleton<CameraHelper>
{
    [SerializeField]
    private Camera mainCamera = null;

    public Camera MainCamera => mainCamera;
}
