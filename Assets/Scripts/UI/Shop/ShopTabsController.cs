using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Shop
{
    public class ShopTabsController : MonoBehaviour
    {
        private bool firstTabIsActive;
        
        [SerializeField] private GameObject cubeDemo;
        [SerializeField] private GameObject characterDemo;

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
                    break;
                case "Cubes":
                    tempDifference = Mathf.Abs(cubesPanel.anchoredPosition.x - centerPosX);
                    charactersPanel.DOAnchorPosX(charactersPanel.anchoredPosition.x + tempDifference, 0.5f);
                    cubesPanel.DOAnchorPosX(cubesPanel.anchoredPosition.x + tempDifference, 0.5f);
                    break;
            }
        }
    }
}