using System;
using DefaultNamespace;
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

        if (other.collider.CompareTag("Hole"))
        {
            boxController.RemoveBox(gameObject, false, 1, true);
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

    private void OnTriggerEnter(Collider other)
    {
        
        if (!transform.CompareTag("DiamondCollector"))
            return;
        
        if (other.CompareTag("Turn"))
        {
            var playerMover = transform.parent.GetComponent<PlayerMover>();
            var playerTurnListener = transform.parent.GetComponent<PlayerTurnListener>();
            
            playerMover.DisableMoving();
            
            var rotationObserver = other.transform.parent.GetChild(0).GetComponent<RotationOberver>();
            playerMover.transform.SetParent(rotationObserver.transform);
            rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(true);
            //playerTurnListener.Subscribe(rotationObserver);
        }
    }
}