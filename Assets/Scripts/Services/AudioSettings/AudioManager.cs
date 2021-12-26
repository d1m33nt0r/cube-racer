using System;
using UnityEngine;

namespace DefaultNamespace.Services.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public bool soundIsEnabled;
        public BoxesAudioSource boxesAudioSource;
        public DiamondAudioSource diamondAudioSource;
        public UIAudioSource uiAudioSource;
        public FlagEffectAudioSource flagEffectAudioSource;
        public BonusAudioSource bonusAudioSource;
        public NitroAudioEffectSource nitroAudioEffectSource;
        
        public void Setup()
        {
            soundIsEnabled = GetSoundSettings();
            boxesAudioSource.SetSoundSettings(soundIsEnabled);
            uiAudioSource.SetSoundSettings(soundIsEnabled);
            diamondAudioSource.SetSoundSettings(soundIsEnabled);
            flagEffectAudioSource.SetSoundSettings(soundIsEnabled);
            bonusAudioSource.SetSoundSettings(soundIsEnabled);
            nitroAudioEffectSource.SetSoundSettings(soundIsEnabled);
        }

        private bool GetSoundSettings()
        {
            if (PlayerPrefs.HasKey("sound_enable"))
                return Convert.ToBoolean(PlayerPrefs.GetInt("sound_enable"));
            
            PlayerPrefs.SetInt("sound_enable", 1);
            return true;
        }
        
        public bool SwitchSoundSettings()
        {
            soundIsEnabled = !soundIsEnabled;
            PlayerPrefs.SetInt("sound_enable", Convert.ToInt16(soundIsEnabled));
            Setup();
            
            return soundIsEnabled;
        }
    }
}