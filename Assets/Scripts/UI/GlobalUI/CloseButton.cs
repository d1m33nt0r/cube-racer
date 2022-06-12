using DefaultNamespace.Services.AudioManager;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.GlobalUI
{
    public class CloseButton : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        private LevelProgressManager levelProgressManager;
        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, LevelProgressManager levelProgressManager, AudioManager _audioManager)
        {
            this.sceneLoader = sceneLoader;
            this.levelProgressManager = levelProgressManager;
            m_audioManager = _audioManager;
        }

        public void SetLoadingParams()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            sceneLoader.SetNextScene(levelProgressManager.GetCurrentLevelString());
            SceneManager.LoadScene("Loader");
        }
    }
}