using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private string nextScene = "";
    private LevelProgressManager levelProgressManager;
    
    
    
    public SceneLoader(LevelProgressManager levelProgressManager)
    {
        this.levelProgressManager = levelProgressManager;
        SetNextScene();
    }

    public void SetNextScene(string nextScene = null)
    {
        if (nextScene != null)
        {
            this.nextScene = nextScene;
            return;
        }

        if (levelProgressManager.GetCurrentLevelString() != "")
            this.nextScene = levelProgressManager.GetCurrentLevelString();
        else
            this.nextScene = "Level_0";
    }

    public AsyncOperation LoadNextScene(bool currentScene = false)
    {
        if (!currentScene)
           return SceneManager.LoadSceneAsync(nextScene);
        else
           return SceneManager.LoadSceneAsync(levelProgressManager.GetCurrentLevelString());
    }
}
