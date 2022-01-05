using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode][Serializable]
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

        public void CLearConnectionPoints()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var faceDisplaceComp = child.GetComponent<FaceDisplace>();
                var customSnapComp = child.GetComponent<CustomSnap>();
                var rigidBodyComp = child.GetComponent<Rigidbody>();
                
                if (faceDisplaceComp != null) DestroyImmediate(faceDisplaceComp);
                if (customSnapComp != null) DestroyImmediate(customSnapComp);
                if (rigidBodyComp != null) DestroyImmediate(rigidBodyComp);
                
                while (child.childCount > 0) DestroyImmediate(transform.GetChild(i).GetChild(0).gameObject);
            }
        }
    }
}