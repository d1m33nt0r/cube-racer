using System;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurveTool
{
    [ExecuteAlways][Serializable]
    public class BezierCurve : MonoBehaviour
    {
        public List<Point> arcs = new List<Point>();
        private Vector3 previousTransformPosition;

        private void OnDrawGizmosSelected()
        {
            for (var i = 0; i < arcs.Count - 1; i++)
            {
                if (arcs[i].isFirstPoint)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(arcs[i].position, 0.1f);
            
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(arcs[i + 1].position, 0.1f);
            
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(arcs[i].handles[0], 0.1f);
            
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(arcs[i + 1].handles[0], 0.1f);
                }

                if (!arcs[i].isFirstPoint)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(arcs[i].position, 0.1f);
            
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(arcs[i + 1].position, 0.1f);
            
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(arcs[i].handles[1], 0.1f);
            
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawSphere(arcs[i + 1].handles[0], 0.1f);
                }
            }
        }

        private void Update()
        {
            if (previousTransformPosition == transform.position) return;

            var bias = GetBias(previousTransformPosition, transform.position);

            foreach (var arc in arcs)
            {
                arc.position += bias;
                for (var i = 0; i < arc.handles.Count; i++)
                    arc.handles[i] += bias;
            }
            
            previousTransformPosition = transform.position;
        }

        private Vector3 GetBias(Vector3 _previousPosition, Vector3 _currentPosition)
        {
            var absValue = new Vector3(Mathf.Abs(_previousPosition.x - _currentPosition.x), 
                Mathf.Abs(_previousPosition.y - _currentPosition.y), 
                Mathf.Abs(_previousPosition.z - _currentPosition.z));

            var result = new Vector3();
            
            if (_previousPosition.x > _currentPosition.x)
                result.x = absValue.x * -1;
            else
                result.x = absValue.x;
            
            if (_previousPosition.y > _currentPosition.y)
                result.y = absValue.y * -1;
            else
                result.y = absValue.y;
            
            if (_previousPosition.z > _currentPosition.z)
                result.z = absValue.z * -1;
            else
                result.z = absValue.z;
            
            return result;
        }
    }
}
