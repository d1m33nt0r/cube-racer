using System;
using UI;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
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
            Camera.main.transform.GetChild(1).transform.SetParent(null);
            FinishGame();
            
            boxController.transform.GetComponent<PlayerEffector>().ActivateDiamondEffect();
            boxController.ClearBoxes();
        }
        
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        StartedGame?.Invoke();
        //m_DiamondUI.DisableSettingsButton();
    }

    public void FinishGame()
    {
        Debug.Log("Game Finished");
        FinishedGame?.Invoke();
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        PausedGame?.Invoke();
    }

    public void FailGame()
    {
        Debug.Log("Game Failed");
        FailedGame?.Invoke();
    }

    public void ContinueGame()
    {
        Debug.Log("Game Continued");
        ContinuedGame?.Invoke();
    }
}