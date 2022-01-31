using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.ObjectPool.Editor
{
    [CustomEditor(typeof(PoolManager))]
    public class PoolManagerInspector : UnityEditor.Editor
    {
        private PoolManager m_poolManager;

        private string m_poolObjectName;
        private int m_poolLength;
        private GameObject m_poolObject;

        private Dictionary<string, bool> m_foldedPools;
        
        private void OnEnable() 
        {
            m_poolManager = (PoolManager) target;
            Initialize();
        }

        private void Initialize()
        {
            m_foldedPools = new Dictionary<string, bool>();

            foreach (var key in m_poolManager.pools.Keys)
                m_foldedPools.Add(key, false);
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginVertical(new GUIStyle{fixedWidth = 150});
            GUILayout.Label("Pool name");
            m_poolObjectName = EditorGUILayout.TextField(m_poolObjectName);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical(new GUIStyle{fixedWidth = 62});
            GUILayout.Label("Pool size");
            m_poolLength = EditorGUILayout.IntField(m_poolLength);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Pool object");
            m_poolObject = (GameObject) EditorGUILayout.ObjectField(m_poolObject, typeof(GameObject), true);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            
            if (GUILayout.Button("Create new pool"))
            {
                var pool = new GameObject(m_poolObjectName);
                pool.transform.SetParent(m_poolManager.transform);
                pool.AddComponent<Pool>();
                pool.GetComponent<Pool>().Initialize(m_poolLength, m_poolObject);

                if (!m_poolManager.pools.ContainsKey(m_poolObjectName))
                {
                    m_poolManager.pools.Add(m_poolObjectName, pool.GetComponent<Pool>());
                    Initialize();
                    return;
                }
            }
            
            EditorGUILayout.EndVertical();
            
            DrawPools();
        }

        private void DrawPools()
        {
            foreach (var keyValue in m_poolManager.pools)
            {
                m_foldedPools[keyValue.Key] = EditorGUILayout.BeginFoldoutHeaderGroup(m_foldedPools[keyValue.Key], keyValue.Key);

                if (m_foldedPools[keyValue.Key])
                {
                    EditorGUILayout.BeginVertical();

                    var poolValue = keyValue.Value;
                    var i = 0;

                    if (GUILayout.Button("Remove this pool"))
                    {
                        m_poolManager.RemovePoolByKey(keyValue.Key);
                        return;
                    }

                    foreach (var t in poolValue.GetComponent<Pool>().objects)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label((i + 1).ToString());
                        GUILayout.Label(t.name);

                        if (GUILayout.Button("Remove"))
                        {
                            var temp = m_poolManager.pools[keyValue.Key].objects[i];
                            m_poolManager.pools[keyValue.Key].objects.RemoveAt(i);
                            DestroyImmediate(temp);
                            return;
                        }
                        
                        EditorGUILayout.EndHorizontal();
                        i++;
                    }
                    
                    EditorGUILayout.EndVertical();
                }
                
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}