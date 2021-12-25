using UnityEngine;

namespace DefaultNamespace.Services.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public bool soundIsEnabled = true;
        public BoxesAudioSource boxesAudioSource;
        public DiamondAudioSource diamondAudioSource;
        public UIAudioSource uiAudioSource;
        public FlagEffectAudioSource flagEffectAudioSource;
        public BonusAudioSource bonusAudioSource;
        public NitroAudioEffectSource nitroAudioEffectSource;
        
        public void Setup()
        {
            boxesAudioSource.SetSoundSettings(soundIsEnabled);
            uiAudioSource.SetSoundSettings(soundIsEnabled);
            diamondAudioSource.SetSoundSettings(soundIsEnabled);
            flagEffectAudioSource.SetSoundSettings(soundIsEnabled);
            bonusAudioSource.SetSoundSettings(soundIsEnabled);
            nitroAudioEffectSource.SetSoundSettings(soundIsEnabled);
        }
        
        public bool SwitchSoundSettings()
        {
            soundIsEnabled = !soundIsEnabled;
            Setup();
            return soundIsEnabled;
        }
    }
}