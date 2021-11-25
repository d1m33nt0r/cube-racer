using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "TestDictionaries", menuName = "TestDictionaries", order = 0)]
    public class TestDictionaries : ScriptableObject
    {
        [SerializeField] private UnitySerializedDictionary<int, string> testDictionary;
    }
}