using System;
using DefaultNamespace;
using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class VibrationButton : MonoBehaviour
    {
        [SerializeField] private Image disableIcon;
        private bool vibrationIsActive;

        private AudioManager m_audioManager;
        private Vibrator m_vibrator;
        
        [Inject]
        private void Construct(AudioManager _audioManager, Vibrator _vibrator)
        {
            m_audioManager = _audioManager;
            m_vibrator = _vibrator;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("vibration_enabled"))
            {
                vibrationIsActive = Convert.ToBoolean(PlayerPrefs.GetInt("vibration_enabled"));

                if (vibrationIsActive)
                {
                    m_vibrator.EnableHaptics();
                }
                else
                {
                    disableIcon.enabled = !disableIcon.enabled;
                    m_vibrator.DisableHaptics();
                }
            }
            else
            {
                vibrationIsActive = true;
                PlayerPrefs.SetInt("vibration_enabled", Convert.ToInt16(vibrationIsActive));
                m_vibrator.EnableHaptics();
            }
        }

        public void SwitchVibrationSettingsStatus()
        {
            vibrationIsActive = !vibrationIsActive;
            disableIcon.enabled = !disableIcon.enabled;
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            PlayerPrefs.SetInt("vibration_enabled", Convert.ToInt16(vibrationIsActive));
            if (vibrationIsActive) m_vibrator.EnableHaptics();
            else m_vibrator.DisableHaptics();
        }
    }
}