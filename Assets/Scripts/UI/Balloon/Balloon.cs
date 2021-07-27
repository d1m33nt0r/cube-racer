using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class Balloon : MonoBehaviour
{
    private int countDiamonds;

    public int CountDiamonds => countDiamonds;

    [SerializeField] private GameObject particleEffect;

    private DiamondUI diamondUI;
    private Animator animator;
    [SerializeField] private Text text;
    
    [Inject]
    private void Construct(DiamondUI diamondUI)
    {
        this.diamondUI = diamondUI;
    }
    
    private void Start()
    {
        animator = GetComponent<Animator>();

        var counts = new List<int>()
        {
            50, 75, 100, 125, 150
        };

        countDiamonds = counts[Random.Range(0, counts.Count - 1)];
        text.text = Convert.ToString(countDiamonds);
    }

    public void PlayAnimationDestroyBalloon()
    {
        animator.Play("DestroyAnimation");
    }

    public void SpawnParticleEffect()
    {
        SpawnDiamonds();
        
        var imageColor = GetComponent<Image>().color;
        var newColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0);
        GetComponent<Image>().color = newColor;
        
        Instantiate(particleEffect).transform.position = new Vector3(transform.position.x, 
            transform.position.y, transform.position.z - 0.2f);
    }

    public void SpawnDiamonds()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            diamondUI
                .CreateDiamondAndMoveWithDelay(Camera.main.WorldToViewportPoint(transform.position), 1f);
        }
    }
}
