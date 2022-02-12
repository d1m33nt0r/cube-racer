using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PhysicsSwitcher : MonoBehaviour
    {
        [SerializeField] private bool start;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DiamondCollector") && start)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, -20, Physics.gravity.z);
            }

            if (other.CompareTag("DiamondCollector") && !start)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, -35, Physics.gravity.z);
            }
        }
    }
}