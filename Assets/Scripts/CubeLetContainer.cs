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

        public Material material;
        public Mesh mesh;
        
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

        private void ChangeMaterialForChildrenObjects()
        {
            if (material == null)
            {
                Debug.LogError("Material is not assigned!");
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial = material;
            }
        }

        private void ChangeMeshForChildrenObjects()
        {
            if (mesh == null)
            {
                Debug.LogError("Mesh is not assigned!");
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh = mesh;
            }
        }

        public void ChangeComponents()
        {
            ChangeMaterialForChildrenObjects();
            ChangeMeshForChildrenObjects();
        }
    }
}