using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

namespace UI
{
    public class DisableAdsButton : MonoBehaviour
    {
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
        }

        public void PressButton()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
        }
    }
}