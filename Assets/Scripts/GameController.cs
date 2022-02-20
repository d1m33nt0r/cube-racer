using System;
using System.Collections;
using DefaultNamespace;
using Firebase.Analytics;
using UI;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    public bool gameStarted;
    public event Action StartedGame;
    public event Action FinishedGame;
    public event Action FailedGame;
    public event Action PausedGame;
    public event Action ContinuedGame;

    private GameplayStarter gameplayStarter;
    private BoxController boxController;
    private SessionDiamondCounter sessionDiamondCounter;
    private DiamondUI m_DiamondUI;
    
    [Inject]
    private void Construct(GameplayStarter gameplayStarter, BoxController boxController, SessionDiamondCounter sessionDiamondCounter, DiamondUI _diamondUI)
    {
        this.gameplayStarter = gameplayStarter;
        this.boxController = boxController;
        this.sessionDiamondCounter = sessionDiamondCounter;
        m_DiamondUI = _diamondUI;
        gameplayStarter.Touched += StartGame;
        boxController.RemovedBox += CheckBoxCount;
    }

    private void CheckBoxCount(bool finish, int multiplier)
    {
        if (boxController.boxCount == 0 && !finish)
        {
            FailGame();
            boxController.ClearBoxes();
        }
        
        if (boxController.boxCount == 0 && finish)
        {
            FinishGame();
            
            boxController.transform.GetComponent<PlayerEffector>().ActivateDiamondEffect();
            boxController.ClearBoxes();
        }
    }
    
    private IEnumerator SendStartLevelEvent()
    {
        while (!FirebaseAnalyticsInitialize.firebaseReady) yield return null;
        FirebaseAnalytics.LogEvent(FindObjectOfType<Level>().CurrentLevel, "Start", "Start");
    }
    
    private IEnumerator SendFailLevelEvent()
    {
        while (!FirebaseAnalyticsInitialize.firebaseReady) yield return null;
        FirebaseAnalytics.LogEvent(FindObjectOfType<Level>().CurrentLevel, "Fail", "Fail");
    }
    
    private IEnumerator SendFinishLevelEvent()
    {
        while (!FirebaseAnalyticsInitialize.firebaseReady) yield return null;
        FirebaseAnalytics.LogEvent(FindObjectOfType<Level>().CurrentLevel, "Finish", "Finish");
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        StartCoroutine(SendStartLevelEvent());
        gameStarted = true;
        StartedGame?.Invoke();
        //m_DiamondUI.DisableSettingsButton();
    }

    public void FinishGame()
    {
        Debug.Log("Game Finished");
        StartCoroutine(SendFinishLevelEvent());
        gameStarted = false;
        FinishedGame?.Invoke();
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        PausedGame?.Invoke();
    }

    private void FailGame()
    {
        Debug.Log("Game Failed");
        StartCoroutine(SendFailLevelEvent());
        FirebaseAnalytics.LogEvent(FindObjectOfType<Level>().CurrentLevel, "Fail", "Fail");
        FailedGame?.Invoke();
    }

    public void ContinueGame()
    {
        Debug.Log("Game Continued");
        ContinuedGame?.Invoke();
    }
}