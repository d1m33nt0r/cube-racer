﻿using UI;
using UnityEngine;
using Zenject;

public class SwipeController : MonoBehaviour
{
    private bool isDragging, isMobilePlatform;
    private Vector2 tapPoint, swipeDelta, prevSwipeDelta;
    private Vector2 curMousePosition, prevMousePosition = Vector2.zero;
    private GameplayStarter gameplayStarter;
    private bool enabledSwipeController;
    
    public delegate void OnSwipeInput(SwipeType type, float delta);
    public event OnSwipeInput SwipeEvent;

    public enum SwipeType
    {
        LEFT,
        RIGHT
    }

    [Inject]
    private void Construct(GameplayStarter gameplayStarter)
    {
        this.gameplayStarter = gameplayStarter;
        gameplayStarter.ActivateSwipeControll += EnableSwipeController;
    }

    private void EnableSwipeController()
    {
        enabledSwipeController = true;;
    }
    
    private void Awake() 
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            isMobilePlatform = false;
        #else
            isMobilePlatform = true;
        #endif
    }

    private void Update()
    {
        if(!enabledSwipeController)
            return;

        if(!isMobilePlatform)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                tapPoint = Input.mousePosition;
                prevMousePosition = tapPoint;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }
        else
        {
            if(Input.touchCount > 0)
            {
                if(Input.touches[0].phase == TouchPhase.Began)
                {
                    isDragging = true;
                    tapPoint = Input.touches[0].position; 
                    prevMousePosition = tapPoint;
                }
                else if(Input.touches[0].phase == TouchPhase.Canceled ||
                        Input.touches[0].phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
        }
        
        CalculateSwipe();
    }

    private void CalculateSwipe()
    {
        curMousePosition = (Vector2)Input.mousePosition;
        swipeDelta = Vector2.zero;
        
        if (isDragging)
        {
            if (!isMobilePlatform && Input.GetMouseButton(0))
                swipeDelta = (Vector2) Input.mousePosition - prevMousePosition;
            else if (Input.touchCount > 0)
                swipeDelta = Input.touches[0].position - tapPoint;
            
            // TODO fix player position
            if (swipeDelta.x < 0 && swipeDelta != prevSwipeDelta)
            {
                SwipeEvent?.Invoke(SwipeType.LEFT, Mathf.Abs(swipeDelta.x) / 40);
            }

            if (swipeDelta.x > 0 && swipeDelta != prevSwipeDelta)
            {
                SwipeEvent?.Invoke(SwipeType.RIGHT, Mathf.Abs(swipeDelta.x) / 40);
            }

            prevSwipeDelta = swipeDelta;
            prevMousePosition = curMousePosition;  
        }

       
    }

    private void ResetSwipe()
    {
        isDragging = false;
        tapPoint = swipeDelta = prevSwipeDelta = Vector2.zero;
    }
}