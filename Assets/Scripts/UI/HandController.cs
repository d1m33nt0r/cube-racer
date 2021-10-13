using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    [SerializeField] private RectTransform targetLeft;
    [SerializeField] private RectTransform targetRight;
    [SerializeField] private float speed;

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
}
