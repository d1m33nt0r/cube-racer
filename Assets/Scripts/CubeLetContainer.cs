using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    public class CubeLetContainer : MonoBehaviour
    {
        [SerializeField, HideInInspector] private SerializableCubeLetsDictionary letCubes;
        [SerializeField, HideInInspector] private List<int> countCubesInColumn;
        [SerializeField, HideInInspector] private bool isInitialized;
        public List<int> CountCubesInColumn => countCubesInColumn;
        public bool IsInitialized => isInitialized;
        public SerializableCubeLetsDictionary LetCubes => letCubes;
        
        public void Initialize()
        {
            countCubesInColumn = new List<int>();
            letCubes = new SerializableCubeLetsDictionary();
            InitializeCubesCountList();
            isInitialized = true;
        }

        private void InitializeCubesCountList()
        {
            for (var i = 0; i < 5; i++)
            {
                countCubesInColumn.Add(0);
                letCubes.Add(i, new List<GameObject>());
            }
        }
    }
}