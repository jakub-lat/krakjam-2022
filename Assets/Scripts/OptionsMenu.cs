using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cyberultimate;
using Cyberultimate.Unity;

public class OptionsMenu : MonoSingleton<OptionsMenu>
{
    private const string MouseSensitivityKey = "MouseSensitivity";
    
    [SerializeField]
    private Dropdown qualityDropdown = null;

    [SerializeField]
    private Text sensitivityText = null;

    [SerializeField]
    private Slider sensitivitySlider = null;

    public static float SensitivityMouse { get; set; } = 0.2f;
    public static event Action<float> OnChangedSensitivity = delegate { };

    protected void Start()
    {
        SensitivityMouse = PlayerPrefs.HasKey(MouseSensitivityKey) ? PlayerPrefs.GetFloat(MouseSensitivityKey) : SensitivityMouse;
        
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        sensitivitySlider.value = SensitivityMouse;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetSensitivity(float newSans) // Undertale reference
    {
        sensitivityText.text = newSans.ToString("0.00");
        SensitivityMouse = newSans;
        PlayerPrefs.SetFloat(MouseSensitivityKey, SensitivityMouse);
    }
}
