using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagEffectSpawner : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private GameObject effect;

    public void CreateEffect()
    {
        Instantiate(effect, point.position, Quaternion.identity, point);
        
    }
}
