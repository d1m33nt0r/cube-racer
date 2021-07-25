using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.Shop
{
    public class ShopButton : MonoBehaviour
    {
        private SceneLoader sceneLoader;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        public void SetLoadingParams()
        {
            sceneLoader.SetNextScene("Shop");
            SceneManager.LoadScene("Loader");
        }
    }
}