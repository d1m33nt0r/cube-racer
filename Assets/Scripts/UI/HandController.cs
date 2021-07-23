using System;
using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public event Action Touched;
    
    [SerializeField] private RectTransform targetLeft;
    [SerializeField] private RectTransform targetRight;
    [SerializeField] private float speed;
    
    private bool touched;
    private bool isMobilePlatform;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        isMobilePlatform = false;
#else
        isMobilePlatform = true;
#endif
    }

    private void Start()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {
        var rectTransform = (transform as RectTransform);
        rectTransform.DOAnchorMax(targetLeft.anchorMax, speed);
        rectTransform.DOAnchorMin(targetLeft.anchorMin, speed);

        rectTransform
            .DOAnchorPos(targetLeft.anchoredPosition, speed)
            .OnComplete(MoveRight);
    }

    private void MoveRight()
    {
        var rectTransform = (transform as RectTransform);
        rectTransform.DOAnchorMax(targetRight.anchorMax, speed);
        rectTransform.DOAnchorMin(targetRight.anchorMin, speed);

        rectTransform
            .DOAnchorPos(targetRight.anchoredPosition, speed)
            .OnComplete(MoveLeft);
    }
    
    private void Update()
    {
        if (touched) return;

        if (!isMobilePlatform)
        {
            if(Input.GetMouseButtonDown(0))
            {
                touched = true;
                Touched?.Invoke();
                Destroy(transform.parent.gameObject);
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                touched = true;
                Touched?.Invoke();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
