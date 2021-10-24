using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchingCylinderRoad : MonoBehaviour
{
    [SerializeField] private bool batchingEnabled;
    void Awake()
    {
        if (batchingEnabled) StaticBatchingUtility.Combine(gameObject);
    }
}
