﻿using UnityEngine;

namespace DefaultNamespace
{
    public class Turn : MonoBehaviour
    {
        [SerializeField] private Transform P0;
        [SerializeField] private Transform P1;
        [SerializeField] private Transform P2;
        [SerializeField] private Transform P3;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private bool moving;
        [SerializeField] private SwipeController swipeController;
        
        [Range(0, 1)] [SerializeField] private float t;
        [Range(-0.4f, 0.4f)] [SerializeField] private float t2;

        private float minLimiter = -0.4f, maxLimiter = 0.4f;
        private Vector3 centerPoint;
        private float previousZ, previousX;

        private float prevDeltaLeft, prevDeltaRight;
        
        private void Start()
        {
            centerPoint = transform.position;
            swipeController.SwipeEvent += Action;
        }

        private void Action(SwipeController.SwipeType swipeType, float delta)
        {
            if (swipeType == SwipeController.SwipeType.LEFT)
            {
                
                t2 = Mathf.Clamp(t2 + prevDeltaLeft - delta, minLimiter, maxLimiter);
                prevDeltaLeft = delta;

                if (prevDeltaRight > 0)
                    prevDeltaLeft = 0;
                
            }
            else
            {
                t2 = Mathf.Clamp(t2 - prevDeltaRight + delta, minLimiter, maxLimiter);
                prevDeltaRight = delta;

                if (prevDeltaLeft > 0)
                    prevDeltaRight = 0;
            }
        }
        
        private void Update()
        {
            transform.position = new Vector3(centerPoint.x + t2, transform.position.y,
                centerPoint.z + t2);
            
            if (moving)
            {
                targetTransform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, t);
                targetTransform.rotation = Quaternion
                    .LookRotation(Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, t));
            }
        }

        public void SetMoving(bool moving)
        {
            this.moving = moving;
        }

        private void OnDrawGizmos() 
        {
            if (P0 == null)
                return;

            var sigmentsNumber = 20;
            var preveousePoint = P0.position;

            for (var i = 0; i < sigmentsNumber + 1; i++) {
                var paremeter = (float)i / sigmentsNumber;
                var point = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, paremeter);
                Debug.DrawLine(preveousePoint, point, Color.black);
                preveousePoint = point;
            }
        }
    }
}