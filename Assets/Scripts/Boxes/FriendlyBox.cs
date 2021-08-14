using System;
using System.Collections;
using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject plusOne;
    
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
            
            var effect2 = Instantiate(effect);
            effect2.transform.position = effectPosition;
            effect2.transform.SetParent(transform);

            var effectText = Instantiate(plusOne);
            effectText.transform.position =
                new Vector3(effectPosition.x, effectPosition.y, effectPosition.z);
            effectText.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            effectText.transform.SetParent(transform);
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
                playerMover.SetCurrentDirection();
            }
        }
    }
}