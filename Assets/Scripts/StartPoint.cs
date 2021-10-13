using UnityEngine;

namespace DefaultNamespace
{
    public static class StartPoint
    {
        public static Vector3 startPoint;

        public static void SetStartPoint(Vector3 _startPoint)
        {
            startPoint = _startPoint;
        }
    }
}