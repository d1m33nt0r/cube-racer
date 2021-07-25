using DefaultNamespace;
using Services.LevelProgressManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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

        if (levelProgressManager.GetData() != "")
            this.nextScene = levelProgressManager.GetData();
        else
            this.nextScene = "Level_0";
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}
