using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cyberultimate;
using Cyberultimate.Unity;
using Options;
using UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoSingleton<OptionsMenu>
{

    [SerializeField]
    private Dropdown qualityDropdown = null;

    [SerializeField]
    private Text sensitivityText = null;

    [SerializeField]
    private Slider sensitivitySlider = null;

    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundSlider;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider voiceSlider;

    [SerializeField]
    private Text musicText;
    [SerializeField]
    private Text soundText;
    [SerializeField]
    private Text masterText;
    [SerializeField]
    private Text voiceText;


    private OptionsManager options;

    protected void Start()
    {
        options = OptionsManager.Current;
        
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        sensitivitySlider.value = options.MouseSensitivity.Value;
        musicSlider.value = options.MusicVolume.Value;
        soundSlider.value = options.SoundVolume.Value;
        masterSlider.value = options.MasterVolume.Value;
        voiceSlider.value = options.VoiceVolume.Value;

        sensitivityText.text = PercentFormat(options.MouseSensitivity.Value);
        musicText.text = PercentFormat(options.MusicVolume.Value);
        soundText.text = PercentFormat(options.SoundVolume.Value);
        masterText.text = PercentFormat(options.MasterVolume.Value);
        voiceText.text = PercentFormat(options.VoiceVolume.Value);
    }

     
    private string PercentFormat(float val) => $"{Mathf.RoundToInt(val * 100)}";
    

    public void SetSensitivity(float newSans) // Undertale reference
    {
        sensitivityText.text = PercentFormat(newSans);
        options.MouseSensitivity.Set(newSans);
    }

    public void SetMusicVolume(float value)
    {
        musicText.text = PercentFormat(value);
        options.MusicVolume.Set(value);
    }

    public void SetSoundVolume(float value)
    {
        soundText.text = PercentFormat(value);
        options.SoundVolume.Set(value);
    }

    public void SetVoiceVolume(float value)
    {
        voiceText.text = PercentFormat(value);
        options.VoiceVolume.Set(value);
    }

    public void SetMasterVolume(float value)
    {
        masterText.text = PercentFormat(value);
        options.MasterVolume.Set(value);
    }

    public void SetQuality(int value)
    {
        qualityDropdown.value = value;
        options.Quality.Set(value);
    }
}
