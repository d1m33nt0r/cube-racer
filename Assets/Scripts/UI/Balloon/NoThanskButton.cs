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
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, LevelProgressManager levelProgressManager)
        {
            this.sceneLoader = sceneLoader;
            this.levelProgressManager = levelProgressManager;
        }
        
        public void LoadNextLevel()
        {
            var level = levelProgressManager.GetData();
            sceneLoader.SetNextScene(level);
            SceneManager.LoadScene("Loader");
        }
    }
}