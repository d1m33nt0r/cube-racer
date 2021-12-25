using DefaultNamespace.Services.AudioManager;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.Balloon
{
    public class NoThanskButton : MonoBehaviour
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
        
        public void LoadNextLevel()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            var level = levelProgressManager.GetData();
            sceneLoader.SetNextScene(level);
            SceneManager.LoadScene("Loader");
        }
    }
}