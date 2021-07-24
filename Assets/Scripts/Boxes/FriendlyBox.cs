using System;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    private BoxController boxController;
    private BoxAudioController boxAudioController;
    
    [Inject] 
    private void Construct(BoxController boxController, BoxAudioController boxAudioController)
    { 
        this.boxController = boxController;
        this.boxAudioController = boxAudioController;
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
            boxController.RemoveBox(gameObject);
            boxController.EnablePhysics(true);
            boxAudioController.PlayFailSound();
        }

        if (other.collider.CompareTag("LevelFinish") && transform.CompareTag("DiamondCollector"))
        {
            other.collider.tag = "Ground";
            boxController.RemoveBox(gameObject);
            boxAudioController.PlayFailSound();
        }
        
        if (other.collider.CompareTag("Ground"))
        {
            boxController.UpdateTrailPosition();
        }
    }
}