using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform directionPoint;
        [SerializeField] private SwipeController _swipeController;

        private float prevDeltaRight, prevDeltaLeft = 0;
        private float minMoveLimiter, maxMoveLimiter;
        
        private Vector3 direction => directionPoint.TransformPoint(directionPoint.position) -
                                     transform.TransformPoint(transform.position);

        private void Start()
        {
            _swipeController.SwipeEvent += Action;
            minMoveLimiter = transform.position.x - 0.5f;
            maxMoveLimiter = transform.position.x + 0.5f;
            
            Debug.Log("Start position: " + transform.position);
        }

        public void Rotate(float angle)
        {
            transform.rotation = Quaternion.AngleAxis(angle - transform.rotation.eulerAngles.y, Vector3.up);
        }

        private void Action(SwipeController.SwipeType swipeType, float delta)
        {
            if (swipeType == SwipeController.SwipeType.LEFT)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + prevDeltaLeft - delta, minMoveLimiter, maxMoveLimiter), transform.position.y, transform.position.z);
                Debug.Log("P position: " + transform.position);
                prevDeltaLeft = delta;

                if (prevDeltaRight > 0)
                    prevDeltaRight = 0;
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x - prevDeltaRight + delta, minMoveLimiter, maxMoveLimiter), transform.position.y, transform.position.z);
                Debug.Log("P position: " + transform.position);
                prevDeltaRight = delta;

                if (prevDeltaLeft > 0)
                    prevDeltaLeft = 0;
            }
        }
        
        private void Update()
        {
            Debug.DrawRay(transform.position, direction, Color.yellow);
            transform.parent.Translate(direction * Time.deltaTime * _speed);
            //_meshScaner.Move(transform.position);
        }
    }
}