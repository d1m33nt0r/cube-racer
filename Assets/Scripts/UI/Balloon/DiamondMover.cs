using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class DiamondMover : MonoBehaviour
{
    private RectTransform diamondIconRectTransform;

    private DiamondUI diamondUI;
    
    [Inject]
    private void Construct(DiamondUI diamondUI)
    {
        this.diamondUI = diamondUI;
    }
    
    void OnEnable()
    {
        diamondIconRectTransform = GetComponent<RectTransform>();
        var diamondIconController = diamondIconRectTransform.GetComponent<DiamondIconController>();
        StartCoroutine(Move(diamondIconController, 0.5f));
    }

    private IEnumerator Move(DiamondIconController diamondIconController, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        diamondIconRectTransform.DOAnchorMax(diamondUI.DiamondPanel.anchorMax, 0.5f);
        diamondIconRectTransform.DOAnchorMin(diamondUI.DiamondPanel.anchorMin, 0.5f);
        
        diamondIconRectTransform
            .DOAnchorPos(diamondUI.DiamondPanel.anchoredPosition, 0.5f)
            .OnComplete(diamondIconController.SetMovingDone);
        
    }
}
