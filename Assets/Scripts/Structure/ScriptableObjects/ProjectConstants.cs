using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Structure.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectConstants", menuName = "Project Settings/Constants", order = 0)]
    public class ProjectConstants<T> : ScriptableObject
    {
        [SerializeField] private Dictionary<string, T> constants;
    }
}