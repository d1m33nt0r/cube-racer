using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PhysicsSwitcher : MonoBehaviour
    {
        private float defaultPhysicsY = -35;
        [SerializeField] private float physicsY = -20;
        [SerializeField] private bool start;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DiamondCollector") && start)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, physicsY, Physics.gravity.z);
            }

            if (other.CompareTag("DiamondCollector") && !start)
            {
                Physics.gravity = new Vector3(Physics.gravity.x, defaultPhysicsY, Physics.gravity.z);
            }
        }
    }
}