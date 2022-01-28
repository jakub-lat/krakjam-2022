using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cyberultimate;
using Cyberultimate.Unity;

public class OptionsMenu : MonoSingleton<OptionsMenu>
{
    [SerializeField]
    private Dropdown qualityDropdown = null;

    [SerializeField]
    private Text sensitivityText = null;

    [SerializeField]
    private Slider sensitivitySlider = null;

    public static float SensitivityMouse { get; set; } = 0.5f;
    public static event Action<float> OnChangedSensitivity = delegate { };

    protected void Start()
    {
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        sensitivitySlider.value = SensitivityMouse;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetSensitivity(float newSans) // Undertale reference
    {
        sensitivityText.text = newSans.ToString();
        SensitivityMouse = newSans;
    }
}
