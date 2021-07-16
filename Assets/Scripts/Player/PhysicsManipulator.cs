using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PhysicsManipulator : MonoBehaviour
    {
        [SerializeField] private float delay;
        
        private List<Rigidbody> rigidbodies = new List<Rigidbody>();
        private bool working;

        public void AddRigidBody(Rigidbody rigidbody)
        {
            rigidbodies.Add(rigidbody);
        }

        public void RemoveRigidBody(Rigidbody rigidbody)
        {
            rigidbodies.Remove(rigidbody);
        }

        public void ClearRigidBodies()
        {
            rigidbodies.Clear();
        }

        public void EnablePhysics()
        {
            if (!working)
                StartCoroutine(EnablePhysicsWithDelay(delay));
        }

        private IEnumerator EnablePhysicsWithDelay(float delay)
        {
            working = true;
            
            for (var i = rigidbodies.Count - 1; i >= 0; i--)
            {
                yield return new WaitForSeconds(delay);
                
                rigidbodies[i].useGravity = true;
            }

            working = false;
        }
    }
}