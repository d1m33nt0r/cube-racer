using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform directionPoint;
        [SerializeField] private SwipeController _swipeController;
        private Vector3 direction => directionPoint.TransformPoint(directionPoint.position) -
                                     transform.TransformPoint(transform.position);

        private void Start()
        {
            _swipeController.SwipeEvent += Action;
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
                transform.position = new Vector3(-delta, transform.position.y, transform.position.z);
                Debug.Log("P position: " + transform.position);
            }
            else
            {
                transform.position = new Vector3(delta, transform.position.y, transform.position.z);
                Debug.Log("P position: " + transform.position);
            }
        }
        
        private void Update()
        {
            Debug.DrawRay(transform.position, direction, Color.yellow);
            transform.Translate(direction * Time.deltaTime * _speed);
            //_meshScaner.Move(transform.position);
        }
    }
}