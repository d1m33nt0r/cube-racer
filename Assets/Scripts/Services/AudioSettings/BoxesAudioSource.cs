using UnityEngine;

namespace DefaultNamespace.Services.AudioManager
{
    public class BoxesAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip[] m_collectAudioClip;
        [SerializeField] private AudioClip m_failAudioClip;
        
        private bool m_soundIsEnabled;
        
        public void SetSoundSettings(bool _soundIsEnabled)
        {
            m_soundIsEnabled = _soundIsEnabled;
        }
        
        public void PlayCollectSound()
        {
            m_audioSource.clip = m_collectAudioClip[Random.Range(0, 1)];
            
            if (!m_soundIsEnabled) return;
            
            m_audioSource.Play();
        }

        public void PlayFailSound()
        {
            m_audioSource.clip = m_failAudioClip;
            
            if (!m_soundIsEnabled) return;
            
            m_audioSource.Play();
        }
    }
}