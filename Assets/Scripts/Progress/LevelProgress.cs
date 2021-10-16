using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Progress
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private List<Transform> checkpoints;
        [SerializeField] private List<TurnState.State> _states;
        
        private Dictionary<Transform, TurnState.State> points;
        private float generalDistance = 0;
        private float currentDistance = 0;
        
        private void Start()
        {
            PlayerMover.ChangedPosition += Move;
            
            points = new Dictionary<Transform, TurnState.State>();
            
            var i = 0;
            foreach (var transformPoint in checkpoints)
            {
                points.Add(transformPoint, _states[i]);
                i++;
            }

            i = 0;
            KeyValuePair<Transform, TurnState.State> prevPoint;
            foreach (var point in points)
            {
                if (i == 0)
                {
                    i++;
                    prevPoint = point;
                    continue;
                }
                
                switch (point.Value)
                {
                    case TurnState.State.Forward:
                        generalDistance += Mathf.Abs(point.Key.position.z - prevPoint.Key.position.z);
                        break;
                    case TurnState.State.Left:
                        generalDistance += Mathf.Abs(point.Key.position.x - prevPoint.Key.position.x); 
                        break;
                    case TurnState.State.Right:
                        generalDistance += Mathf.Abs(point.Key.position.x - prevPoint.Key.position.x); 
                        break;
                    case TurnState.State.Back:
                        generalDistance += Mathf.Abs(point.Key.position.z - prevPoint.Key.position.z);
                        break;
                }

                prevPoint = point;
            }
            
            Debug.Log(generalDistance);
        }

        private void Move(Vector3 difference)
        {
            switch (TurnState.state)
            {
                case TurnState.State.Forward:
                    currentDistance += difference.z;
                    break;
                case TurnState.State.Left:
                    currentDistance -= difference.x;
                    break;
                case TurnState.State.Right:
                    currentDistance += difference.x;
                    break;
                case TurnState.State.Back:
                    currentDistance -= difference.z;
                    break;
            }
            
            DebugLog();
        }

        private void DebugLog()
        {
            Debug.Log(currentDistance);
        }
    }
}