using System.Collections.Generic;
using DefaultNamespace.Boxes;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(BoxGroup)), CanEditMultipleObjects]
    public class BoxGroupInspector : Editor
    {
        private BoxGroup targetObject;
        private Vector3 startPoint;
        private GameObject friendlyBoxPrefab;
        
        private void OnEnable()
        {
            targetObject = (BoxGroup)target;
            startPoint = targetObject.transform.position;
            friendlyBoxPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/1/Prefabs/Cubes/default_cube.prefab", typeof(GameObject));
        }
        
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add box"))
            {
                if (targetObject.boxes == null) targetObject.boxes = new List<FriendlyBox>();
                var position = new Vector3(startPoint.x, startPoint.y + targetObject.boxes.Count * targetObject.heightBox, startPoint.z);
                var prefab = (GameObject) PrefabUtility.InstantiatePrefab(friendlyBoxPrefab);
                prefab.transform.position = position;
                prefab.transform.rotation = Quaternion.identity;
                prefab.transform.SetParent(targetObject.transform);
                targetObject.boxes.Add(prefab.GetComponent<FriendlyBox>());
                EditorUtility.SetDirty(targetObject);
            }

            if (GUILayout.Button("Remove box"))
            {
                var temp = targetObject.boxes[targetObject.boxes.Count - 1];
                targetObject.boxes.RemoveAt(targetObject.boxes.Count - 1);
                DestroyImmediate(temp);
                EditorUtility.SetDirty(targetObject);
            }

        }
    }
}