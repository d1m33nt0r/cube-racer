using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagEffectSpawner : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private GameObject effect;
    [SerializeField] private Vector3 rotation;
    
    public void CreateEffect()
    {
        Instantiate(effect, point.position, Quaternion.Euler(rotation.x, rotation.y, rotation.z), point);
        
    }
}
