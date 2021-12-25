using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class VibrationButton : MonoBehaviour
    {
        [SerializeField] private Image disableIcon;
        private bool vibrationIsActive = true;

        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
        }
        
        public void SwitchVibrationSettingsStatus()
        {
            vibrationIsActive = !vibrationIsActive;
            disableIcon.enabled = !disableIcon.enabled;
            m_audioManager.uiAudioSource.PlayButtonClickSound();
        }
    }
}