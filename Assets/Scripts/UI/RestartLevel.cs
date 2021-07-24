using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class RestartLevel : MonoBehaviour
    {
        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}