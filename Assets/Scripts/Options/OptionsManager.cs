using System.Reflection;
using Cyberultimate.Unity;
using UnityEngine;
using UnityEngine.Audio;

namespace Options
{
    public class OptionsManager : MonoSingleton<OptionsManager>
    {
        private const string MouseSensitivityKey = "MouseSensitivity";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundVolumeKey = "SoundVolume";
        private const string MasterVolumeKey = "MasterVolume";
        private const string VoiceVolumeKey = "VoiceVolume";
        private const string QualityKey = "GraphicsQuality";
        
        [SerializeField] private AudioMixer mixer;
        
        private static AudioMixer CurrentMixer => Current.mixer;

        public GameOption<float> MouseSensitivity;
        
        public GameOption<float> MusicVolume;

        public GameOption<float> SoundVolume;

        public GameOption<float> MasterVolume;
        
        public GameOption<float> VoiceVolume;

        public GameOption<int> Quality;
        

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            transform.parent = null;
            
            if (mixer == null)
            {
                mixer = Resources.Load("MainMix") as AudioMixer;
            }
            
            MouseSensitivity = new(MouseSensitivityKey, _ => { }, 0.2f);
            
            MusicVolume = new(MusicVolumeKey, value =>
            {
                CurrentMixer.SetFloat(MusicVolumeKey, Mathf.Log10(value) * 20);
            }, 1);
            
            SoundVolume = new(SoundVolumeKey, value =>
            {
                CurrentMixer.SetFloat(SoundVolumeKey, Mathf.Log10(value) * 20);
            }, 1);
            
            MasterVolume = new(MasterVolumeKey, value =>
            {
                CurrentMixer.SetFloat(MasterVolumeKey, Mathf.Log10(value) * 20);
            }, 1);
            
            VoiceVolume = new(VoiceVolumeKey, value =>
            {
                CurrentMixer.SetFloat(VoiceVolumeKey, Mathf.Log10(value) * 20);
            }, 1);
            
            Quality = new(QualityKey, value =>
            {
                QualitySettings.SetQualityLevel(value);
            }, QualitySettings.GetQualityLevel());

        }
    }
}
