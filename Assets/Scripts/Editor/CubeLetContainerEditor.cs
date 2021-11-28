using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(CubeLetContainer)), CanEditMultipleObjects]
    public class CubeLetContainerEditor : Editor
    {
        private GameObject cubeLetPrefab;
        private Vector3 startPoint;
        private Vector3 LeftLeftPoint => new Vector3(startPoint.x - 0.4f, startPoint.y, startPoint.z);
        private Vector3 LeftPoint => new Vector3(startPoint.x - 0.2f, startPoint.y, startPoint.z);
        private Vector3 RightPoint => new Vector3(startPoint.x + 0.2f, startPoint.y, startPoint.z);
        private Vector3 RightRightPoint => new Vector3(startPoint.x + 0.4f, startPoint.y, startPoint.z);
        
        private CubeLetContainer targetObject;
        private float cubeHeight = 0.2105f;
        
        private void OnEnable()
        {
            targetObject = target as CubeLetContainer;
            startPoint = targetObject.transform.position;
            cubeLetPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/LetCube.prefab", typeof(GameObject));
            if (!(targetObject is null) && !targetObject.IsInitialized) Initialize();
        }

        private void Initialize()
        {
            targetObject.Initialize();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginVertical();
            targetObject.material = (Material) EditorGUILayout.ObjectField(targetObject.material, typeof(Material), true);
            targetObject.mesh = (Mesh) EditorGUILayout.ObjectField(targetObject.mesh, typeof(Mesh), true);
            if (GUILayout.Button("Change graphics"))
            {
                targetObject.ChangeComponents();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginHorizontal();
            var i = 0;
            foreach (var count in targetObject.CountCubesInColumn)
            {
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("+"))
                {
                    AddCubeInColumnByIndex(i);
                }
                EditorGUILayout.IntField(count, style);
                if (GUILayout.Button("-"))
                {
                    RemoveCubeInColumnBuIndex(i);
                }
                EditorGUILayout.EndVertical();
                i++;
            }
            EditorGUILayout.EndHorizontal();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void AddCubeInColumnByIndex(int _columnIndex)
        {
            GameObject prefab;
            Vector3 position;
            switch (_columnIndex)
            {
                case 0:
                    position = new Vector3(LeftLeftPoint.x, LeftLeftPoint.y + targetObject.LetCubes[_columnIndex].Count * cubeHeight, LeftLeftPoint.z);
                    prefab = Instantiate(cubeLetPrefab, position, Quaternion.identity, targetObject.transform);
                    targetObject.LetCubes[_columnIndex].Add(prefab);
                    break;
                case 1:
                    position = new Vector3(LeftPoint.x, LeftPoint.y + targetObject.LetCubes[_columnIndex].Count * cubeHeight, LeftPoint.z);
                    prefab = Instantiate(cubeLetPrefab, position, Quaternion.identity, targetObject.transform);
                    targetObject.LetCubes[_columnIndex].Add(prefab);
                    break;
                case 2:
                    position = new Vector3(startPoint.x, startPoint.y + targetObject.LetCubes[_columnIndex].Count * cubeHeight, startPoint.z);
                    prefab = Instantiate(cubeLetPrefab, position, Quaternion.identity, targetObject.transform);
                    targetObject.LetCubes[_columnIndex].Add(prefab);
                    break;
                case 3:
                    position = new Vector3(RightPoint.x, RightPoint.y + targetObject.LetCubes[_columnIndex].Count * cubeHeight, RightPoint.z);
                    prefab = Instantiate(cubeLetPrefab, position, Quaternion.identity, targetObject.transform);
                    targetObject.LetCubes[_columnIndex].Add(prefab);
                    break;
                case 4:
                    position = new Vector3(RightRightPoint.x, RightRightPoint.y + targetObject.LetCubes[_columnIndex].Count * cubeHeight, RightRightPoint.z);
                    prefab = Instantiate(cubeLetPrefab, position, Quaternion.identity, targetObject.transform);
                    targetObject.LetCubes[_columnIndex].Add(prefab);
                    break;
            }
            
            EditorUtility.SetDirty(targetObject);
        }

        private void RemoveCubeInColumnBuIndex(int _columnIndex)
        {
            if (targetObject.LetCubes[_columnIndex].Count == 0) return;
            var temp = targetObject.LetCubes[_columnIndex][targetObject.LetCubes[_columnIndex].Count - 1];
            targetObject.LetCubes[_columnIndex].RemoveAt(targetObject.LetCubes[_columnIndex].Count - 1);
            DestroyImmediate(temp);
            
            EditorUtility.SetDirty(targetObject);
        }
    }
}