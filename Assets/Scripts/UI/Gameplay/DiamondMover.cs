using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMover : MonoBehaviour
{
    [SerializeField] private GameObject diamondImagePrefab;
    [SerializeField] private RectTransform parent;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform diamondIcon;
    
    private RectTransform target;
    
    public float speed = 1.0F;

    private float startTime;
    private float journeyLength;
    
    public void CreateDiamond(Vector2 screenPoint)
    {
        target = Instantiate(diamondImagePrefab, parent).GetComponent<RectTransform>();
        SetStartPosition(screenPoint);
    }

    private void SetStartPosition(Vector2 screenPoint)
    {
        Vector2 anchoredPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, mainCamera,
            out anchoredPosition);
        
        target.anchoredPosition = anchoredPosition;

        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        var targetPosition = target.anchoredPosition;
        
        while (true)
        {
            var distCovered = (Time.time - startTime) * speed;
            
            var fractionOfJourney = distCovered / journeyLength;
            
            Vector2 anchoredPosition;
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Vector2.Lerp(targetPosition, 
                    diamondIcon.anchoredPosition, fractionOfJourney), mainCamera,out anchoredPosition);
        
            target.anchoredPosition = anchoredPosition;

            yield return null;
        }
    }
}
