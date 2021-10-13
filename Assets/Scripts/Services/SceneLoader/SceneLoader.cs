using Services.LevelProgressManager;
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

        if (levelProgressManager.GetData() != "")
            this.nextScene = levelProgressManager.GetData();
        else
            this.nextScene = "Level_0";
    }

    public void LoadNextScene(bool currentScene = false)
    {
        if (!currentScene)
            SceneManager.LoadSceneAsync(nextScene);
        else
            SceneManager.LoadScene(levelProgressManager.GetData());
    }
}
