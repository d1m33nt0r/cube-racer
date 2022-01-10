using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchingCylinderRoad : MonoBehaviour
{
    [SerializeField] private bool batchingEnabled;


    private void Update()
    {
        if (batchingEnabled) StaticBatchingUtility.Combine(gameObject);
    }
}
