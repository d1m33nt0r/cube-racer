﻿using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private bool balloonOnFinished;
        
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject finishUI;
        [SerializeField] private GameObject balloonUI;
        
        [Inject]
        private void Construct(GameController gameController)
        {
            gameController.StartedGame += ShowGameplayUI;
            gameController.StartedGame += HideMainMenu;
            gameController.StartedGame += HideGameOverUI;
            gameController.StartedGame += HideFinishUI;
            
            gameController.FinishedGame += ShowFinish;
            gameController.FailedGame += ShowGameOverUI;
        }

        private void ShowFinish()
        {
            if (balloonOnFinished)
                ShowBalloonUI();
            else
                ShowFinishUI();
            
            GameObject.Find("Main Camera").GetComponent<CameraController>().RotateAround(GameObject.Find("Player").transform);
            //GameObject.Find("Camera").GetComponent<CameraController>().RotateAround(GameObject.Find("Player").transform);
        }
        
        private void ShowBalloonUI()
        {
            balloonUI.SetActive(true);
        }
        
        private void HideBalloonUI()
        {
            balloonUI.SetActive(false);
        }
        
        private void ShowFinishUI()
        {
            finishUI.SetActive(true);
        }

        private void HideFinishUI()
        {
            finishUI.SetActive(false);
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