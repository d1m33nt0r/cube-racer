using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private SwipeController _swipeController;
        [SerializeField] private Transform leftLimiter;
        [SerializeField] private Transform rightLimiter;
        
        private bool movingEnabled;
        private float prevDeltaRight, prevDeltaLeft = 0;
        private GameController gameController;

        [Inject]
        private void Construct(GameController gameController)
        {
            this.gameController = gameController;
        }
        
        private void Start()
        {
            gameController.StartedGame += EnableMoving;
            gameController.FailedGame += DisableMoving;
            gameController.FinishedGame += DisableMoving;

            SubscribeSwipes();
        }

        private void EnablePhysics()
        {
            transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        }
        
        private void DisablePhysics()
        {
            transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        
        public void EnableMoving()
        {
            movingEnabled = true;
            SubscribeSwipes();
        }
        
        public void DisableMoving()
        {
            movingEnabled = false;
            UnsubscribeSwipes();
            EnablePhysics();
        }

        private void Action(SwipeController.SwipeType swipeType, float delta)
        {
            if (swipeType == SwipeController.SwipeType.LEFT)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x + prevDeltaLeft - delta, 
                    leftLimiter.position.x, rightLimiter.position.x), transform.position.y, transform.position.z);
                prevDeltaLeft = delta;

                if (prevDeltaRight > 0)
                    prevDeltaRight = 0;
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x - prevDeltaRight + delta, 
                    leftLimiter.position.x, rightLimiter.position.x), transform.position.y, transform.position.z);
                prevDeltaRight = delta;

                if (prevDeltaLeft > 0)
                    prevDeltaLeft = 0;
            }
        }

        private void SubscribeSwipes()
        {
            _swipeController.SwipeEvent += Action;
        }
        
        private void UnsubscribeSwipes()
        {
            _swipeController.SwipeEvent -= Action;
        }
        
        private void Update()
        {
            if (movingEnabled)
                transform.parent.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
    }
}