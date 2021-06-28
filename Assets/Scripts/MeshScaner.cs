using System;
using System.Collections;
using UnityEngine;

public class MeshScaner : MonoBehaviour
{
    public void Move(Vector3 targetPosition)
    {
        transform.position = new Vector3(targetPosition.x, targetPosition.y - 0.6f, targetPosition.z);
    }
}