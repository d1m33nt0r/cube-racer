using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopTabsController : MonoBehaviour
    {
        private bool firstTabIsActive;

        [SerializeField] private Sprite activeNavBut;
        [SerializeField] private Sprite inactiveNavBut;
        
        [SerializeField] private Sprite activeRightBut2;
        [SerializeField] private Sprite inactiveRightBut2;
        
        [SerializeField] private Sprite activeLeftBut2;
        [SerializeField] private Sprite inactiveLeftBut2;
        
        [SerializeField] private GameObject cubeDemo;
        [SerializeField] private GameObject characterDemo;

        [SerializeField] private Button cubesButton;
        [SerializeField] private Button characterButton;

        [SerializeField] private Button cubesBottomNavigateButton;
        [SerializeField] private Button charactersBottomNavigateButton;

        [SerializeField] private RectTransform cubesPanel;
        [SerializeField] private RectTransform charactersPanel;

        private float centerPosX;
        private float tempDifference;

        private void Start()
        {
            centerPosX = cubesPanel.anchoredPosition.x;
            tempDifference = charactersPanel.anchoredPosition.x - centerPosX;
        }

        public void SwitchTab(string tabName)
        {
            switch (tabName)
            {
                case "Characters":
                    tempDifference = Mathf.Abs(charactersPanel.anchoredPosition.x - centerPosX);
                    charactersPanel.DOAnchorPosX(charactersPanel.anchoredPosition.x - tempDifference, 0.5f);
                    cubesPanel.DOAnchorPosX(cubesPanel.anchoredPosition.x - tempDifference, 0.5f);
                    DisableCubes();
                    EnableCharacters();
                    break;
                case "Cubes":
                    tempDifference = Mathf.Abs(cubesPanel.anchoredPosition.x - centerPosX);
                    charactersPanel.DOAnchorPosX(charactersPanel.anchoredPosition.x + tempDifference, 0.5f);
                    cubesPanel.DOAnchorPosX(cubesPanel.anchoredPosition.x + tempDifference, 0.5f);
                    DisableCharacters();
                    EnableCubes();
                    break;
            }
        }

        private void DisableCubes()
        {
            cubeDemo.SetActive(false);
            cubesBottomNavigateButton.image.sprite = inactiveNavBut;
            cubesButton.image.sprite = inactiveLeftBut2;
        }

        private void DisableCharacters()
        {
            characterDemo.SetActive(false);
            charactersBottomNavigateButton.image.sprite = inactiveNavBut;
            characterButton.image.sprite = inactiveRightBut2;
        }

        private void EnableCubes()
        {
            cubeDemo.SetActive(true);
            cubesBottomNavigateButton.image.sprite = activeNavBut;
            cubesButton.image.sprite = activeLeftBut2;
        }

        private void EnableCharacters()
        {
            characterDemo.SetActive(true);
            charactersBottomNavigateButton.image.sprite = inactiveNavBut;
            characterButton.image.sprite = activeRightBut2;
        }
    }
}