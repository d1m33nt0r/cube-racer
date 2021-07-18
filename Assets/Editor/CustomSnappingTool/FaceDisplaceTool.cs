using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FaceDisplace))]
public class FaceDisplaceTool : Editor
{
    private FaceDisplace faceDisplace;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var faceDisplace = (FaceDisplace) target;

        if (GUILayout.Button("Create Connectors"))
        {
            faceDisplace.Displace();
        }
        
        if (GUILayout.Button("Remove Connectors"))
        {
            faceDisplace.RemoveConnectors();
        }
    }
}
