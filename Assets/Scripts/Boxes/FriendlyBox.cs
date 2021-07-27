using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    private BoxController boxController;
    private BoxAudioController boxAudioController;
    private ThemeManager themeManager;
    
    [Inject] 
    public void Construct(BoxController boxController, BoxAudioController boxAudioController, ThemeManager themeManager, GameController gameController)
    { 
        this.boxController = boxController;
        this.boxAudioController = boxAudioController;
        this.themeManager = themeManager;
        
        gameController.FinishedGame += () =>
        {
            Destroy(gameObject);
        };
    }

    private void Start()
    {
        GetCurrentMaterial();
    }

    private void GetCurrentMaterial()
    {
        GetComponent<Renderer>().material = themeManager.GetTheme();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!transform.parent || transform.parent.name != "Player")
            return;

        if (other.collider.CompareTag("Untagged"))
            return;
            
        if (other.collider.CompareTag("FriendlyBox"))
        {
            boxController.AddBox(other.gameObject);
            boxController.DisablePhysics();
            boxAudioController.PlayCollectSound();
        }

        if (other.collider.CompareTag("LetBox") && Mathf.Abs(other.transform.position.y - transform.position.y) < 0.1f)
        {
            boxController.RemoveBox(gameObject, false, 1);
            boxController.EnablePhysics(true);
            boxAudioController.PlayFailSound();
        }

        if (other.collider.CompareTag("LevelFinish") && transform.CompareTag("DiamondCollector"))
        {
            other.collider.tag = "Ground";
            boxController.RemoveBox(gameObject, true, 1);
            boxAudioController.PlayFailSound();
        }
        
        if (other.collider.CompareTag("Ground"))
        {
            boxController.UpdateTrailPosition();
        }
    }
}