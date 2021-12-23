using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SamoletMover : MonoBehaviour
    {
        private void Update()
        {
            transform.Translate(Vector3.right * 0.5f);
        }
    }
}