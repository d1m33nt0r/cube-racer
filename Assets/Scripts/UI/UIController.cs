using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject GameOverUI;
        [SerializeField] private GameObject HandUI;
        [SerializeField] private GameObject DiamondUI;

        private GameController gameController;
        
        [Inject]
        private void Construct(GameController gameController)
        {
            this.gameController = gameController;
           
        }
        
        private void ShowGameOverUI()
        {
            GameOverUI.SetActive(true);
        }

        private void HideGameOverUI()
        {
            GameOverUI.SetActive(false);
        }
        
        private void ShowHandUI()
        {
            HandUI.SetActive(true);
        }

        private void HideHandUI()
        {
            HandUI.SetActive(false);
        }
        
        private void ShowDiamondUI()
        {
            DiamondUI.SetActive(true);
        }

        private void HideDiamondUI()
        {
            DiamondUI.SetActive(false);
        }
    }
}