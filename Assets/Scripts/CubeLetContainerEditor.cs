using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(CubeLetContainer))]
    public class CubeLetContainerEditor : Editor
    {
        private CubeLetContainer targetObject;
        
        private void OnEnable()
        {
            targetObject = target as CubeLetContainer;
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
            
            EditorGUILayout.BeginHorizontal();
            foreach (var count in targetObject.CountCubesInColumn)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Button("+");
                EditorGUILayout.IntField(count, style);
                GUILayout.Button("-");
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void AddCubeInColumnByIndex(int _index)
        {
           // targetObject.LetCubes[_index].Add();
        }
    }
}