using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace
{
    public class RotationController : MonoBehaviour
    {
        private bool rot;
        private Transform towards;
        private float duration;
        private bool dop;

        private float rotationY;
        
        private void Update()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity) && !dop)
            {
                rotationY = transform.rotation.y;
                rot = true;
                towards = hit.transform;
                dop = true;
            }

            if (rot)
            {
                transform.RotateAround(towards.position, Vector3.up, -180 * Time.deltaTime);
                if (Mathf.Abs(rotationY - transform.rotation.y) > 90)
                    rot = false;

            }
        }
    }
}