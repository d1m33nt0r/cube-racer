using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        public List<GameObject> objects;
        public int minCountThreshold;
        public GameObject target;
        
        private Queue<GameObject> m_objects;

        public void Initialize(int _countObjects, GameObject _target)
        {
            objects = new List<GameObject>();
            target = _target;
            
            for (int i = 0; i < _countObjects; i++)
            {
                var instance = Instantiate(target, transform, true);
                instance.SetActive(false);
                objects.Add(instance);
            }
        }

        private void Start()
        {
            m_objects = new Queue<GameObject>();

            foreach (var obj in objects)
                m_objects.Enqueue(obj);
        }

        private void ExpandPool(int _count)
        {
            for (int i = 0; i < _count; i++)
            {
                var instance = Instantiate(target, transform, true);
                instance.SetActive(false);
                objects.Add(instance);
                m_objects.Enqueue(instance);
            }
        }
        
        public GameObject GetObject()
        {
            if (m_objects.Count == 0) ExpandPool(1);
            return m_objects.Dequeue();
        }

        public void ReleaseObject(GameObject _gameObject)
        {
            _gameObject.SetActive(false);
            _gameObject.transform.SetParent(transform);
            m_objects.Enqueue(_gameObject);
        }
    }
}