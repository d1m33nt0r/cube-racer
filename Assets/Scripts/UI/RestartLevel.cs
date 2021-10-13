using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class RestartLevel : MonoBehaviour
    {
        private DiamondCountManager diamondCountManager;

        [Inject]
        private void Construct(DiamondCountManager diamondCountManager)
        {
            this.diamondCountManager = diamondCountManager;
        }

        public void ReloadCurrentScene()
        {
            diamondCountManager.WriteData();
            
            SceneManager.LoadScene("Loader");
        }
    }
}