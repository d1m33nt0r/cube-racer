using DefaultNamespace;
using DefaultNamespace.Services.AudioManager;
using Services.DiamondCountManager;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private bool balloon;
        
        private LevelProgressManager levelProgressManager;
        private DiamondCountManager diamondCountManager;
        private SceneLoader sceneLoader;
        private Level level;

        private AudioManager m_audioManager;
        
        [Inject]
        private void Construct(LevelProgressManager levelProgressManager, 
            DiamondCountManager diamondCountManager, SceneLoader sceneLoader,
            Level level, AudioManager _audioManager)
        {
            this.levelProgressManager = levelProgressManager;
            this.diamondCountManager = diamondCountManager;
            this.sceneLoader = sceneLoader;
            this.level = level;
            m_audioManager = _audioManager;
        }
        
        public void LoadNextLevel()
        {
            m_audioManager.uiAudioSource.PlayButtonClickSound();
            levelProgressManager.UpdateData(level.NextLevel);
            levelProgressManager.WriteData();
            diamondCountManager.WriteData();
            
            if (balloon)
                sceneLoader.SetNextScene("Balloon");
            else
                sceneLoader.SetNextScene(level.NextLevel);
            
            SceneManager.LoadScene("Loader");
        }
    }
}