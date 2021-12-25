using UnityEngine;

namespace DefaultNamespace.Services.AudioManager
{
    public class FlagEffectAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;

        private bool m_soundIsEnabled;

        public void SetSoundSettings(bool _soundIsEnabled)
        {
            m_soundIsEnabled = _soundIsEnabled;
        }
        
        public void PlayFlagEffectSound()
        {
            if (!m_soundIsEnabled) return;
            
            m_audioSource.Play();
        }
    }
}