using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    private static Camera mainCamera = null;

    protected void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    public static Camera MainCamera => mainCamera;
}
