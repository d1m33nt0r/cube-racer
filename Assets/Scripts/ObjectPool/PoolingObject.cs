using System;
using UnityEngine;

namespace DefaultNamespace.ObjectPool
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolingObject : MonoBehaviour, IPoolingObject
    {
        private Pool m_pool;

        private void Start()
        {
            m_pool = GetComponentInParent<Pool>();
        }
        
        public void ReturnToPool()
        {
            m_pool.ReleaseObject(gameObject);
        }

        private void OnParticleSystemStopped()
        {
            ReturnToPool();
        }
    }
}