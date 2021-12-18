using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraDebug))]
public class CameraDebugInspector : Editor
{
    private CameraDebug cameraDebug;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cameraDebug = (CameraDebug) target;

        if (GUILayout.Button("Scale field view"))
        {
            cameraDebug.ScaleFieldView();
        }
    }
}