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
        private float cubeHeight = 0.20f;
        
        private void OnEnable()
        {
            targetObject = target as CubeLetContainer;
            startPoint = targetObject.transform.position;
            cubeLetPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/New/Prefabs/let_cube.prefab", typeof(GameObject));
            if (!targetObject.isInitialized) Initialize();
        }

        private void CalculateCubes()
        {
            var i = 0;
            foreach (var cubesValue in targetObject.letCubes.Values)
            {
                targetObject.countCubesInColumn[i] = cubesValue.cubes.Count;
                i++;
            }
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
            
            if (GUILayout.Button("Refresh counters"))
            {
                CalculateCubes();
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginHorizontal();
            var i = 0;
            foreach (var count in targetObject.countCubesInColumn)
            {
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("+"))
                {
                    AddCubeInColumnByIndex(i);
                    return;
                }
                EditorGUILayout.IntField(count, style);
                if (GUILayout.Button("-"))
                {
                    RemoveCubeInColumnBuIndex(i);
                    return;
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
                    position = new Vector3(LeftLeftPoint.x, LeftLeftPoint.y + targetObject.letCubes[_columnIndex].cubes.Count * cubeHeight, LeftLeftPoint.z);
                    prefab = (GameObject) PrefabUtility.InstantiatePrefab(cubeLetPrefab);
                    prefab.transform.position = position;
                    prefab.transform.rotation = Quaternion.identity;
                    prefab.transform.SetParent(targetObject.transform);
                    targetObject.letCubes[_columnIndex].cubes.Add(prefab);
                    break;
                case 1:
                    position = new Vector3(LeftPoint.x, LeftPoint.y + targetObject.letCubes[_columnIndex].cubes.Count * cubeHeight, LeftPoint.z);
                    prefab = (GameObject) PrefabUtility.InstantiatePrefab(cubeLetPrefab);
                    prefab.transform.position = position;
                    prefab.transform.rotation = Quaternion.identity;
                    prefab.transform.SetParent(targetObject.transform);
                    targetObject.letCubes[_columnIndex].cubes.Add(prefab);
                    break;
                case 2:
                    position = new Vector3(startPoint.x, startPoint.y + targetObject.letCubes[_columnIndex].cubes.Count * cubeHeight, startPoint.z);
                    prefab = (GameObject) PrefabUtility.InstantiatePrefab(cubeLetPrefab);
                    prefab.transform.position = position;
                    prefab.transform.rotation = Quaternion.identity;
                    prefab.transform.SetParent(targetObject.transform);
                    targetObject.letCubes[_columnIndex].cubes.Add(prefab);
                    break;
                case 3:
                    position = new Vector3(RightPoint.x, RightPoint.y + targetObject.letCubes[_columnIndex].cubes.Count * cubeHeight, RightPoint.z);
                    prefab = (GameObject) PrefabUtility.InstantiatePrefab(cubeLetPrefab);
                    prefab.transform.position = position;
                    prefab.transform.rotation = Quaternion.identity;
                    prefab.transform.SetParent(targetObject.transform);
                    targetObject.letCubes[_columnIndex].cubes.Add(prefab);
                    break;
                case 4:
                    position = new Vector3(RightRightPoint.x, RightRightPoint.y + targetObject.letCubes[_columnIndex].cubes.Count * cubeHeight, RightRightPoint.z);
                    prefab = (GameObject) PrefabUtility.InstantiatePrefab(cubeLetPrefab);
                    prefab.transform.position = position;
                    prefab.transform.rotation = Quaternion.identity;
                    prefab.transform.SetParent(targetObject.transform);
                    targetObject.letCubes[_columnIndex].cubes.Add(prefab);
                    break;
            }

            targetObject.countCubesInColumn[_columnIndex]++;
            
            EditorUtility.SetDirty(targetObject);
        }

        private void RemoveCubeInColumnBuIndex(int _columnIndex)
        {
            if (targetObject.letCubes[_columnIndex].cubes.Count == 0) return;
            var temp = targetObject.letCubes[_columnIndex].cubes[targetObject.letCubes[_columnIndex].cubes.Count - 1];
            targetObject.letCubes[_columnIndex].cubes.RemoveAt(targetObject.letCubes[_columnIndex].cubes.Count - 1);
            DestroyImmediate(temp);
            targetObject.countCubesInColumn[_columnIndex]--;
            EditorUtility.SetDirty(targetObject);
        }
    }
}