using System;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurveTool
{
    [Serializable]
    public class Point
    {
        public bool isFirstPoint;
        public Vector3 position;
        public List<Vector3> handles = new List<Vector3>();
        public bool isUpArc;
        
        public Point(bool _isFirstPoint, Vector3 _position, List<Vector3> _handles, bool _isUpArc)
        {
            isFirstPoint = _isFirstPoint;
            position = _position;
            isUpArc = _isUpArc;
            handles = _handles;
        }
    }
}