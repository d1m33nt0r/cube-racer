using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameplayStarter : Button
    {
        public event Action Touched;
        public event Action ActivateSwipeControll;

        private bool touched;

        private bool isMobilePlatform;
        private Vector2 tapPoint, swipeDelta;

        private bool hz;
        
        private void Awake()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            isMobilePlatform = false;
#else
        isMobilePlatform = true;
#endif
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            ActivateSwipeControll?.Invoke();
            tapPoint = Input.mousePosition;
            hz = true;
        }

        private void Update()
        {
            if (touched) return;

            if (!hz) return;

            swipeDelta = Vector2.zero;
            
            if (!isMobilePlatform && Input.GetMouseButton(0))
                swipeDelta = (Vector2) Input.mousePosition - tapPoint;
            else if (Input.touchCount > 0)
                swipeDelta = Input.touches[0].position - tapPoint;

            if (swipeDelta.x < 0 || swipeDelta.x > 0)
            {
                Touched?.Invoke();
                touched = true;
            }
        }
    }
}