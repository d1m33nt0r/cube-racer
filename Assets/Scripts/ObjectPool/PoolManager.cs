using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] public SerializablePool pools = new SerializablePool();

        public GameObject GetObject(string _key) => pools[_key].GetObject();

        public void RemovePoolByKey(string key)
        {
            if (pools.ContainsKey(key))
            {
                var value = pools[key];
                pools.Remove(new KeyValuePair<string, Pool>(key, value));
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == key)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    return;
                }
            }
        }
    }
}