using UnityEngine;

namespace DefaultNamespace
{
    public class CenterPointGetter : MonoBehaviour
    {
        [SerializeField] private Transform center;

        public Vector3 GetCenterPoint()
        {
            return center.position;
        }
    }
}