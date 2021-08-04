using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    
    private BoxController boxController;
    private BoxAudioController boxAudioController;
    private ThemeManager themeManager;

    [Inject] 
    public void Construct(BoxController boxController, BoxAudioController boxAudioController, 
        ThemeManager themeManager, GameController gameController)
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
            var previousCountBoxes = boxController.boxCount;
            boxController.AddBox(other.gameObject);
            var currentCountBoxes = boxController.boxCount;
            boxController.DisablePhysics();
            boxAudioController.PlayCollectSound();
            var effectPosition = new Vector3(other.transform.position.x, 
                other.transform.position.y + boxController.height * previousCountBoxes, other.transform.position.z);
            Instantiate(effect).transform.position = effectPosition;
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
            boxController.EnablePhysics();
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
            var rotationObserver = other.transform.parent.GetChild(0).GetComponent<RotationOberver>();

            if (other.name == "StartTrigger")
            {
                playerMover.DisableMoving();
                playerMover.transform.parent.SetParent(rotationObserver.transform);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(true);
            }

            if (other.name == "FinishTrigger")
            {
                playerMover.EnableMoving();
                playerMover.transform.parent.SetParent(null);
                rotationObserver.transform.parent.GetComponent<Turn>().SetMoving(false);
                playerMover.transform.parent.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }
}