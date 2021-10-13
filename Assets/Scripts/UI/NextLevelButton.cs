using DefaultNamespace;
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

        [Inject]
        private void Construct(LevelProgressManager levelProgressManager, 
            DiamondCountManager diamondCountManager, SceneLoader sceneLoader,
            Level level)
        {
            this.levelProgressManager = levelProgressManager;
            this.diamondCountManager = diamondCountManager;
            this.sceneLoader = sceneLoader;
            this.level = level;
        }
        
        public void LoadNextLevel()
        {
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