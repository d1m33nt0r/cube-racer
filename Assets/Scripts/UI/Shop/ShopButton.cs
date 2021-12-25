using DefaultNamespace.Services.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.Shop
{
    public class ShopButton : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        private AudioManager m_audioManger;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, AudioManager _audioManager)
        {
            m_audioManger = _audioManager;
            this.sceneLoader = sceneLoader;
        }

        public void SetLoadingParams()
        {
            m_audioManger.uiAudioSource.PlayButtonClickSound();
            sceneLoader.SetNextScene("Shop");
            SceneManager.LoadScene("Loader");
        }
    }
}