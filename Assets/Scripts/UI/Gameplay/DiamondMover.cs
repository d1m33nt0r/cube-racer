using UnityEngine;
using DG.Tweening;

public class DiamondMover : MonoBehaviour
{
    [SerializeField] private GameObject diamondIconPrefab;
    [SerializeField] private RectTransform parent;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform target;
    
    private RectTransform diamondIconRectTransform;
    
    public void CreateDiamond(Vector2 screenPoint)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        SetStartPosition(screenPoint);
        
        var diamondIconController = diamondIconRectTransform.GetComponent<DiamondIconController>();

        diamondIconRectTransform.DOAnchorMax(target.anchorMax, 0.8f);
        diamondIconRectTransform.DOAnchorMin(target.anchorMin, 0.8f);
        
        diamondIconRectTransform
            .DOAnchorPos(target.anchoredPosition, 0.8f)
            .OnComplete(diamondIconController.SetMovingDone);
    }
    
    private void SetStartPosition(Vector2 screenPoint)
    {
        Vector2 anchoredPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, mainCamera,
            out anchoredPosition);
        
        diamondIconRectTransform.anchoredPosition = anchoredPosition;
    }
}
