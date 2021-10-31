using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class PlayerEffector : MonoBehaviour
{
    [SerializeField] private GameObject diamondEffect;
    [SerializeField] private Vibrator vibrator;
    
    [Inject]
    private void Construct(Vibrator _vibrator)
    {
        vibrator = _vibrator;
    }

    public void ActivateDiamondEffect()
    {
        diamondEffect.SetActive(true);
        
        vibrator.VibrateDiamond();
    }
}
