using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class BonusEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effect;

        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
        }
        
        public void Play()
        {
            effect.Play();
            m_audioManager.bonusAudioSource.PlayBonusAudioSound();
        }
    }
}