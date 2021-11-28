using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class PlayerMover : MonoBehaviour
    {
        public static event Action<Vector3> ChangedPosition;
        
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float speed;
        [SerializeField] private SwipeController swipeController;
        [SerializeField] private Transform leftLimiter;
        [SerializeField] private Transform rightLimiter;
        [SerializeField] private ParticleSystem nitroEffect;

        private bool movingEnabled;
        private float prevDeltaRight, prevDeltaLeft = 0;
        private GameController gameController;

        private Vector3 prevPos, curPos;
        
        [Inject]
        private void Construct(GameController gameController)
        {
            this.gameController = gameController;
        }

        public void IncreaseSpeed(float miltiplierSpeed)
        {
            speed += miltiplierSpeed;
            nitroEffect.Play();
        }

        public void SetDefaultSpeed()
        {
            speed = defaultSpeed;
            nitroEffect.Stop();
        }
        
        private void Start()
        {
            gameController.StartedGame += EnableMoving;
            gameController.FailedGame += DisableMoving;
            gameController.FinishedGame += DisableMoving;
            
            SubscribeSwipes();
        }

        public void SetCurrentDirection()
        {
            if (TurnState.state == TurnState.State.Forward)
                transform.parent.rotation = Quaternion.AngleAxis(0, Vector3.up);
            else if(TurnState.state == TurnState.State.Left)
                transform.parent.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            else if(TurnState.state == TurnState.State.Right)
                transform.parent.rotation = Quaternion.AngleAxis(+90, Vector3.up);
            else if(TurnState.state == TurnState.State.Back)
                transform.parent.rotation = Quaternion.AngleAxis(-180, Vector3.up);
            //EnableMoving();
            //SubscribeSwipes();
        }
        
        public void EnablePhysics()
        {
            transform.GetChild(1).GetComponent<Rigidbody>().useGravity = true;
        }
        
        public void DisablePhysics()
        {
            transform.GetChild(1).GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(1).GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        
        public void EnableMoving()
        {
            movingEnabled = true;
        }
        
        public void DisableMoving()
        {
            movingEnabled = false;
            EnablePhysics();
        }

        private void Action(SwipeController.SwipeType swipeType, float delta)
        {
            if (swipeType == SwipeController.SwipeType.LEFT)
                LeftSwipe(delta);
            else
                RightSwipe(delta);
        }

        private void LeftSwipe(float delta)
        {
            var clampedCoordinate = Mathf.Clamp(transform.localPosition.x + prevDeltaLeft - delta,
                leftLimiter.localPosition.x, rightLimiter.localPosition.x);
            transform.localPosition = new Vector3(clampedCoordinate, transform.localPosition.y, transform.localPosition.z);
            
            /*
            float clampedCoordinate;
            switch (TurnState.state)
            {
                case TurnState.State.Forward:
                    clampedCoordinate = Mathf.Clamp(transform.position.x + prevDeltaLeft - delta,
                        leftLimiter.position.x, rightLimiter.position.x);
                    transform.position = new Vector3(clampedCoordinate, transform.position.y, transform.position.z);
                    break;
                case TurnState.State.Left:
                    clampedCoordinate = Mathf.Clamp(transform.position.z + prevDeltaLeft - delta, 
                        leftLimiter.position.z, rightLimiter.position.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, clampedCoordinate);
                    break;
                case TurnState.State.Right:
                    clampedCoordinate = Mathf.Clamp(transform.localPosition.x - prevDeltaLeft + delta, 
                        leftLimiter.localPosition.x, rightLimiter.localPosition.x);
                    transform.position = new Vector3(clampedCoordinate, transform.position.y, clampedCoordinate);
                    break;
                case TurnState.State.Back:
                    
                    break;
            }
            */

            prevDeltaLeft = delta;

           
            prevDeltaRight = 0;
        }

        private void RightSwipe(float delta)
        {
            var clampedCoordinate = Mathf.Clamp(transform.localPosition.x - prevDeltaRight + delta,
                leftLimiter.localPosition.x, rightLimiter.localPosition.x);
            transform.localPosition = new Vector3(clampedCoordinate, transform.localPosition.y, transform.localPosition.z);
            
            /*
            float clampedCoordinate;
            switch (TurnState.state)
            {
                case TurnState.State.Forward:
                    clampedCoordinate = Mathf.Clamp(transform.position.x - prevDeltaRight + delta,
                        leftLimiter.position.x, rightLimiter.position.x);
                    transform.position = new Vector3(clampedCoordinate, transform.position.y, transform.position.z);
                    break;
                case TurnState.State.Left:
                    clampedCoordinate = Mathf.Clamp(transform.position.z - prevDeltaRight + delta,
                        leftLimiter.position.z, rightLimiter.position.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, clampedCoordinate);
                    break;
                case TurnState.State.Right:
                    clampedCoordinate = Mathf.Clamp(transform.position.z + prevDeltaRight - delta,
                        leftLimiter.position.z, rightLimiter.position.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, clampedCoordinate);
                    break;
                case TurnState.State.Back:
                    
                    break;
            }
            */

            prevDeltaRight = delta;

            
            prevDeltaLeft = 0;
        }

        public void CustomMove(Vector3 positionXZ, Quaternion rotation)
        {
            transform.parent.position = new Vector3(positionXZ.x, transform.parent.position.y, positionXZ.z);
            transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, rotation.eulerAngles.y, transform.parent.rotation.z);
        }
        
        public void SubscribeSwipes()
        {
            swipeController.SwipeEvent += Action;
        }
        
        public void UnsubscribeSwipes()
        {
            swipeController.SwipeEvent -= Action;
        }
        
        private void Update()
        {
            if (!movingEnabled) return;

            prevPos = transform.parent.position;
            
            transform.parent.Translate(Vector3.forward * Time.deltaTime * speed);

            curPos = transform.parent.position;
            
            ChangedPosition?.Invoke(curPos - prevPos);
        }
    }
}