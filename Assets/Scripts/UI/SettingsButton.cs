using UnityEngine;
using Zenject;

namespace UI
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private bool status;
        
        public void Construct(GameController _gameController)
        {
            _gameController.StartedGame += PlayHideAnimation;
        }

        public void ButtonPress()
        {
            status = !status;
            
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