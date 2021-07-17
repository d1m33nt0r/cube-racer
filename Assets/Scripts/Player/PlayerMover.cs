﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
 
        [SerializeField] private SwipeController _swipeController;

        private const float moveLimiter = 0.4f;
        
        private float prevDeltaRight, prevDeltaLeft = 0;
        private float minMoveLimiter, maxMoveLimiter;
        


        private void Start()
        {
            _swipeController.SwipeEvent += Action;
            minMoveLimiter = transform.position.x - moveLimiter;
            maxMoveLimiter = transform.position.x + moveLimiter;
        }

        public void Rotate(float angle)
        {
            transform.rotation = Quaternion.AngleAxis(angle - transform.rotation.eulerAngles.y, Vector3.up);
        }

        private void Action(SwipeController.SwipeType swipeType, float delta)
        {
            if (swipeType == SwipeController.SwipeType.LEFT)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + prevDeltaLeft - delta, 
                    minMoveLimiter, maxMoveLimiter), transform.position.y, transform.position.z);
                prevDeltaLeft = delta;

                if (prevDeltaRight > 0)
                    prevDeltaRight = 0;
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x - prevDeltaRight + delta, 
                    minMoveLimiter, maxMoveLimiter), transform.position.y, transform.position.z);
                prevDeltaRight = delta;

                if (prevDeltaLeft > 0)
                    prevDeltaLeft = 0;
            }
        }
        
        private void Update()
        {
            Debug.DrawRay(transform.position, Vector3.forward, Color.yellow);
            transform.parent.Translate(Vector3.forward * Time.deltaTime * _speed);
            //_meshScaner.Move(transform.position);
        }
    }
}