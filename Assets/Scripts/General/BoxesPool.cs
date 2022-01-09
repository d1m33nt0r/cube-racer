using UnityEngine;

namespace DefaultNamespace.General
{
    public class BoxesPool : MonoBehaviour
    {
        [SerializeField] private GameObject[] boxes;
        
        public GameObject[] Boxes => boxes;
    }
}