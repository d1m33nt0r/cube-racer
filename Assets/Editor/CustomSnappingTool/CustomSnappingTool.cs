using System;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

[EditorTool("Custom Snap Move", typeof(CustomSnap))]
public class CustomSnappingTool : EditorTool
{
    public Texture2D ToolIcon;

    private Transform oldTarget;
    private CustomSnapConnector[] allPoints;
    private CustomSnapConnector[] targetPoints;

    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = ToolIcon,
                text = "Custom Snap Move Tool",
                tooltip = "Custom Snap Move Tool"
            };
        }
    }

    public override void OnToolGUI(EditorWindow window)
    {
        var targetTransform = ((CustomSnap) target).transform;

        if (targetTransform != oldTarget)
        {
            var prefabStage = PrefabStageUtility.GetPrefabStage(targetTransform.gameObject);

            if (prefabStage != null)
                allPoints = prefabStage.prefabContentsRoot.GetComponentsInChildren<CustomSnapConnector>();
            else
                allPoints = FindObjectsOfType<CustomSnapConnector>();
            
            targetPoints = targetTransform.GetComponentsInChildren<CustomSnapConnector>();

            oldTarget = targetTransform;
        }
        
        EditorGUI.BeginChangeCheck();

        var newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetTransform, "Move with custom snap");
            MoveWithSnapping(targetTransform, newPosition);
        }
    }

    private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
    {
        var bestPosition = newPosition;

        var closesDistance = float.PositiveInfinity;

        foreach (var point in allPoints)
        {
            if (point.transform.parent == targetTransform) continue;

            foreach (var ownPoint in targetPoints)
            {
                var targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                var distance = Vector3.Distance(targetPos, newPosition);

                if (distance < closesDistance)
                {
                    closesDistance = distance;
                    bestPosition = targetPos;
                }
            }
        }

        if (closesDistance < 0.1f)
        {
            targetTransform.position = bestPosition;
        }
        else
        {
            targetTransform.position = newPosition;
        }
    }
}
