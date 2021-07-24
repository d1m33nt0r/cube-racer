using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameOverUI;
        
        [Inject]
        private void Construct(GameController gameController)
        {
            gameController.StartedGame += ShowGameplayUI;
            gameController.StartedGame += HideMainMenu;
            gameController.StartedGame += HideGameOverUI;
            
            gameController.FailedGame += ShowGameOverUI;
            
        }

        private void ShowGameOverUI()
        {
            gameOverUI.SetActive(true);
        }

        private void HideGameOverUI()
        {
            gameOverUI.SetActive(false);
        }
        
        private void ShowGameplayUI()
        {
            gameplayUI.SetActive(true);
        }

        private void HideGameplayUI()
        {
            gameplayUI.SetActive(false);
        }
        
        private void ShowMainMenu()
        {
            mainMenuUI.SetActive(true);
        }

        private void HideMainMenu()
        {
            mainMenuUI.SetActive(false);
        }
    }
}