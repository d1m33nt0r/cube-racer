using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BezierCurveTool.Editor
{
   
    [CustomEditor(typeof(BezierCurve))]
    public class BezierGUI : UnityEditor.Editor 
    {
        [MenuItem("GameObject/Create Other/Bezier Curve")]
        static void CreateBezierCurve()
        {
            var gameObject = new GameObject
            {
                name = "Bezier Curve", 
                transform = { position = Vector3.zero }
            };
            gameObject.AddComponent<BezierCurve>();
            gameObject.GetComponent<BezierCurve>().arcs.Add(new Point(true, 
                new Vector3(-1, 0, 0), new List<Vector3>{new Vector3(-0.5f, 0, 1)}, true));
            gameObject.GetComponent<BezierCurve>().arcs.Add(new Point(false, 
                new Vector3(1, 0, 0), new List<Vector3>{new Vector3(0.5f, 0, -1)}, true));
        }
        
        public override void OnInspectorGUI()
        {
            var script = (BezierCurve) target;

            if (script.arcs.Count == 0) EditorGUILayout.LabelField("Points is empty");

            if (GUILayout.Button("Add point"))
            {
                var last = script.arcs[script.arcs.Count - 1];
                last.handles.Add(new Vector3(last.handles[0].x + 1, last.handles[0].y, last.handles[0].z * -1));
                script.arcs.Add(new Point(false, 
                        new Vector3(last.position.x + 2, last.position.y, last.position.z), 
                        new List<Vector3>{new Vector3(last.handles[1].x + 1, last.handles[1].y, last.handles[1].z)}, !last.isUpArc));
            }
        }
        
        void OnSceneGUI() 
        {
            var script = (BezierCurve) target;
            var points = script.arcs;
            
            for (var i = 0; i < points.Count - 1; i++)
            {
                if (points[i].isFirstPoint)
                {
                    points[i].position = Handles.PositionHandle(points[i].position, Quaternion.identity);
                    points[i].handles[0] = Handles.PositionHandle(points[i].handles[0], Quaternion.identity);
                    Handles.DrawLine(points[i].position, points[i].handles[0]);
                    points[i + 1].position = Handles.PositionHandle(points[i + 1].position, Quaternion.identity);
                    points[i + 1].handles[0] = Handles.PositionHandle(points[i + 1].handles[0], Quaternion.identity);
                    Handles.DrawLine(points[i + 1].position, points[i + 1].handles[0]);
                }

                if (!points[i].isFirstPoint)
                {
                    points[i].position = Handles.PositionHandle(points[i].position, Quaternion.identity);
                    points[i].handles[1] = Handles.PositionHandle(points[i].handles[1], Quaternion.identity);
                    Handles.DrawLine(points[i].position, points[i].handles[1]);
                    points[i + 1].position = Handles.PositionHandle(points[i + 1].position, Quaternion.identity);
                    points[i + 1].handles[0] = Handles.PositionHandle(points[i + 1].handles[0], Quaternion.identity);
                    Handles.DrawLine(points[i + 1].position, points[i + 1].handles[0]);
                }
            }
            
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i].isFirstPoint)
                {
                    Handles.DrawBezier(points[i].position, points[i + 1].position, points[i].handles[0],
                        points[i + 1].handles[0], Color.red, null, 5);
                }

                if (!points[i].isFirstPoint)
                {
                    Handles.DrawBezier(points[i].position, points[i + 1].position, points[i].handles[1], 
                        points[i + 1].handles[0], Color.red, null, 5);
                }
            }
        }
    }
}