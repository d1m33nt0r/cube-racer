using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class PlayerEffector : MonoBehaviour
{
    [SerializeField] private GameObject diamondEffect;
    
    public void ActivateDiamondEffect()
    {
        diamondEffect.SetActive(true);
    }
}
