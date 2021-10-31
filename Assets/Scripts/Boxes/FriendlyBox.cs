using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ThemeManager;
using UnityEngine;
using Zenject;

public class FriendlyBox : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject plusOne;
    [SerializeField] private bool isBoxBonus;
    [SerializeField] private GameObject boxes;
    [SerializeField] private GameObject lavaEffect;
    
    private BoxController boxController;
    private BoxAudioController boxAudioController;
    private ThemeManager themeManager;
    private Vibrator vibrator;

    [Inject] 
    public void Construct(BoxController boxController, BoxAudioController boxAudioController, 
        ThemeManager themeManager, GameController gameController, Vibrator _vibrator)
    {
        this.boxController = boxController;
        this.boxAudioController = boxAudioController;
        this.themeManager = themeManager;
        vibrator = _vibrator;
    }

    private void Start()
    {
        GetCurrentMaterial();
    }

    private void GetCurrentMaterial()
    {
        GetComponent<MeshFilter>().sharedMesh = themeManager.GetTheme();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isBoxBonus)
        {
            if (!transform.parent || transform.parent.name != "Player")
                return;

            //if (other.collider.CompareTag("FriendlyBox"))
            //{
            //    boxController.AddBox(other.gameObject);
            //    ////boxController.DisablePhysics();
            //    boxAudioController.PlayCollectSound();
           //     SpawnEffects();
           // }

           //if (other.collider.CompareTag("BoxGroup") && transform.CompareTag("DiamondCollector"))
           //{
               
           //}
           
            if (other.collider.CompareTag("LetBox") && Mathf.Abs(other.transform.position.y - transform.position.y) < 0.1f)
            {
                boxController.RemoveBox(gameObject, false, 1);
                //boxController.EnablePhysics(true);
                boxAudioController.PlayFailSound();
            }

            if (other.collider.CompareTag("Hole"))
            {
                Instantiate(lavaEffect, transform.position, Quaternion.identity);
                boxController.RemoveBox(gameObject, false, 1, true);
                //boxController.EnablePhysics(true);
                boxAudioController.PlayFailSound();
                vibrator.VibrateLava();
            }

            if (other.collider.CompareTag("LevelFinish"))
            {
                other.collider.tag = "Ground";
                boxController.RemoveBox(gameObject, true, 1);
                boxController.DisablePhysics();
                boxAudioController.PlayFailSound();
                //boxController.EnablePhysics();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBoxBonus) return;
        
        if (other.CompareTag("DiamondCollector"))
        {
            boxController.SpecialAddBox(5);
            Destroy(gameObject);
        }
    }

    private void SpawnEffects()
    {
        var effectPosition = new Vector3(transform.position.x,
            transform.position.y , transform.position.z);

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
}