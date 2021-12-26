using System;
using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class SoundButton : MonoBehaviour
    {
        [SerializeField] private Image disableIcon;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
        }

        private void Start()
        {
            disableIcon.enabled = !m_audioManager.soundIsEnabled;
        }

        public void SwitchSoundSettingsStatus()
        {
            disableIcon.enabled = !m_audioManager.SwitchSoundSettings();
            m_audioManager.uiAudioSource.PlayButtonClickSound();
        }
    }
}