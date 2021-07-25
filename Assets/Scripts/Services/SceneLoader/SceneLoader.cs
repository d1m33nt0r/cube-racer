using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private string nextScene = "";

    public SceneLoader()
    {
        SetNextScene();
    }
    
    public void SetNextScene(string nextScene = null)
    {
        if (nextScene != null)
        {
            this.nextScene = nextScene;
            return;
        }
        
        if (PlayerPrefs.HasKey("current_level"))
            this.nextScene = PlayerPrefs.GetString("current_level");
        else
            this.nextScene = "Level_0";
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}
