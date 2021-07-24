using System;
using System.Collections;
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
    
    [Inject]
    private void Construct(GameplayStarter gameplayStarter, BoxController boxController)
    {
        this.gameplayStarter = gameplayStarter;
        this.boxController = boxController;
        
        gameplayStarter.Touched += StartGame;
        boxController.BoxCountChanged += CheckBoxCount;
    }

    public IEnumerator ExecuteForWait(float n)
    {
        yield return new WaitForSeconds(n);
        StartGame();
    }

    private void CheckBoxCount()
    {
        if (boxController.boxCount == 0)
        {
            FailGame();
        }
    }

    private void StartGame()
    {
        Debug.Log("Game Started");
        StartedGame?.Invoke();
    }

    private void FinishGame()
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