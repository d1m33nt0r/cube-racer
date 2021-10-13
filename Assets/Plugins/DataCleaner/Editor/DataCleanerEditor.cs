using Plugins.DataCleaner;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(DataCleaner))]
public class DataCleanerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var dataCleaner = (DataCleaner) target;

        if (GUILayout.Button("Clean data"))
        {
            dataCleaner.CleanData();
        }
        
    }
}