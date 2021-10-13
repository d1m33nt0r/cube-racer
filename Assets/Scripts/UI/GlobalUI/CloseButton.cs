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
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, LevelProgressManager levelProgressManager)
        {
            this.sceneLoader = sceneLoader;
            this.levelProgressManager = levelProgressManager;
        }

        public void SetLoadingParams()
        {
            sceneLoader.SetNextScene(levelProgressManager.GetData());
            SceneManager.LoadScene("Loader");
        }
    }
}