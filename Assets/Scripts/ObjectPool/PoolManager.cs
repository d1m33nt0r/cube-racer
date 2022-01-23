using UnityEngine;

namespace DefaultNamespace.ObjectPool
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] public SerializablePool pools = new SerializablePool();

        public GameObject GetObject(string _key) => pools[_key].GetObject();
    }
}