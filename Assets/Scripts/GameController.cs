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
    
    [Inject]
    private void Construct(GameplayStarter gameplayStarter, BoxController boxController, SessionDiamondCounter sessionDiamondCounter)
    {
        this.gameplayStarter = gameplayStarter;
        this.boxController = boxController;
        this.sessionDiamondCounter = sessionDiamondCounter;
        
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
            boxController.ClearBoxes();
        }
        
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        StartedGame?.Invoke();
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