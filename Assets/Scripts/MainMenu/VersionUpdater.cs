using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUpdater : MonoBehaviour
{
    protected void Start()
    {
        Text text = GetComponent<Text>();
        text.text = Application.version;
    }
}
