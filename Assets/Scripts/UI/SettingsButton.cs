using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private AudioManager m_audioManager;
        private bool status;
        
        public void Construct(GameController _gameController)
        {
            _gameController.StartedGame += PlayHideAnimation;
            
        }

        [Inject]
        private void Inject(AudioManager _audioManager)
        {
            m_audioManager = _audioManager;
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += ((_arg0, _scene) =>
            {
                PlayHideAnimation();
            });
        }

        public void ButtonPress()
        {
            status = !status;
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            
            if (status)
                PlayShowAnimation();
            else
                PlayHideAnimation();
        }
        
        private void PlayHideAnimation()
        {
            _animator.Play("HideSettings");
        }

        private void PlayShowAnimation()
        {
            _animator.Play("ShowSettings");
        }
    }
}