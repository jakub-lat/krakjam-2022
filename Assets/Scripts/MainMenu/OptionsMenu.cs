using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cyberultimate;
using Cyberultimate.Unity;
using UnityEngine.Audio;

public class OptionsMenu : MonoSingleton<OptionsMenu>
{
    private const string MouseSensitivityKey = "MouseSensitivity";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";
    private const string MasterVolumeKey = "MasterVolume";
    private const string VoiceVolumeKey = "VoiceVolume";

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

    public static float SensitivityMouse { get; set; } = 0.2f;
    private float _MusicVolume = 1;
    public float MusicVolume { get { return _MusicVolume; } private set 
        { 
            if (value != _MusicVolume)
            {
                mixer.SetFloat(MusicVolumeKey, Mathf.Log10(value) * 20);
                _MusicVolume = value;
            }
        } }

    private float _SoundVolume = 1;
    public float SoundVolume
    {
        get { return _SoundVolume; }
        private set
        {
            if (value != _SoundVolume)
            {
                mixer.SetFloat(SoundVolumeKey, Mathf.Log10(value) * 20);
                _SoundVolume = value;
            }
        }
    }
    private float _MasterVolume = 1;
    public float MasterVolume
    {
        get { return _MasterVolume; }
        private set
        {
            if (value != _MasterVolume)
            {
                mixer.SetFloat(MasterVolumeKey, Mathf.Log10(value) * 20);
                _MasterVolume = value;
            }
        }
    }

    private float _VoiceVolume = 1;
    public float VoiceVolume
    {
        get { return _VoiceVolume; }
        private set
        {
            if (value != _VoiceVolume)
            {
                mixer.SetFloat(VoiceVolumeKey, Mathf.Log10(value) * 20);
                _VoiceVolume = value;
            }
        }
    }

    public static event Action<float> OnChangedSensitivity = delegate { };

    [SerializeField]
    private AudioMixer mixer;

    protected void Start()
    {
        SensitivityMouse = PlayerPrefs.HasKey(MouseSensitivityKey) ? PlayerPrefs.GetFloat(MouseSensitivityKey) : SensitivityMouse;
        MusicVolume = PlayerPrefs.HasKey(MusicVolumeKey) ? PlayerPrefs.GetFloat(MusicVolumeKey) : MusicVolume;
        SoundVolume = PlayerPrefs.HasKey(SoundVolumeKey) ? PlayerPrefs.GetFloat (SoundVolumeKey) : SoundVolume;
        MasterVolume = PlayerPrefs.HasKey(MasterVolumeKey) ? PlayerPrefs.GetFloat(MasterVolumeKey) : MasterVolume;
        VoiceVolume = PlayerPrefs.HasKey(VoiceVolumeKey) ? PlayerPrefs.GetFloat(VoiceVolumeKey) : VoiceVolume;
        
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        sensitivitySlider.value = SensitivityMouse;
        musicSlider.value = MusicVolume;
        soundSlider.value = SoundVolume;
        masterSlider.value = MasterVolume;
        voiceSlider.value = VoiceVolume;
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

    public void SetMusicVolume(float value)
    {
        musicText.text = value.ToString("0.00");
        MusicVolume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
    }

    public void SetSoundVolume(float value)
    {
        soundText.text = value.ToString("0.00");
        SoundVolume = value;
        PlayerPrefs.SetFloat(SoundVolumeKey, SoundVolume);
    }

    public void SetVoiceVolume(float value)
    {
        voiceText.text = value.ToString("0.00");
        VoiceVolume = value;
        PlayerPrefs.SetFloat(VoiceVolumeKey, VoiceVolume);
    }

    public void SetMasterVolume(float value)
    {
        masterText.text = value.ToString("0.00");
        MasterVolume = value;
        PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
    }
}
