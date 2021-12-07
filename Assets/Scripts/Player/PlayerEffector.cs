using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class PlayerEffector : MonoBehaviour
{
    [SerializeField] private GameObject diamondEffect;
    [SerializeField] private GameObject salutEffect;
    
    public void ActivateDiamondEffect()
    {
        diamondEffect.SetActive(true);
        salutEffect.SetActive(true);
    }
}
