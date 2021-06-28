using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action StartedGame;
    public event Action FinishedGame;
    public event Action FailedGame;
    public event Action PausedGame;
    public event Action ContinuedGame;

    private void Start()
    {
        StartGame();
    }

    public IEnumerator ExecuteForWait(float n)
    {
        yield return new WaitForSeconds(n);
        FinishGame();
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