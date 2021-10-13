using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffector : MonoBehaviour
{
    [SerializeField] private GameObject diamondEffect;

    public void ActivateDiamondEffect()
    {
        diamondEffect.SetActive(true);
    }
}
