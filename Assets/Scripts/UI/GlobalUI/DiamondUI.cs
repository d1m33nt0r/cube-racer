using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DiamondUI : MonoBehaviour
{
    [SerializeField] private GameObject diamondIconPrefab;
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform target;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject closeButton;
    
    private RectTransform diamondIconRectTransform;
    private Canvas canvas;

    public RectTransform DiamondPanel => target;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        canvas = GetComponent<Canvas>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Loader")
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
            
            if (scene.name == "Shop")
            {
                settingsButton.SetActive(false);
                closeButton.SetActive(true);
            }
            else if (scene.name == "Balloon")
            {
                settingsButton.SetActive(false);
                closeButton.SetActive(false);
            }
            else
            {
                settingsButton.SetActive(true);
                closeButton.SetActive(false);
            }
        }
    }
    
    public void CreateDiamondAndMoveWithDelay(Vector2 screenPoint, float delay)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        SetStartPosition2(screenPoint);


        var diamondIconController = diamondIconRectTransform.GetComponent<DiamondIconController>();

        StartCoroutine(Move(diamondIconController, delay));
    }

    private IEnumerator Move(DiamondIconController diamondIconController, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        diamondIconRectTransform.DOAnchorMax(target.anchorMax, 2f);
        diamondIconRectTransform.DOAnchorMin(target.anchorMin, 2f);
        
        diamondIconRectTransform
            .DOAnchorPos(target.anchoredPosition, 2f)
            .OnComplete(diamondIconController.SetMovingDone);
        
    }
    
    public void CreateDiamond(Vector2 screenPoint)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        SetStartPosition(screenPoint);
        
        var diamondIconController = diamondIconRectTransform.GetComponent<DiamondIconController>();

        diamondIconRectTransform.DOAnchorMax(target.anchorMax, 0.5f);
        diamondIconRectTransform.DOAnchorMin(target.anchorMin, 0.5f);
        
        diamondIconRectTransform
            .DOAnchorPos(target.anchoredPosition, 0.5f)
            .OnComplete(diamondIconController.SetMovingDone);
    }
    
    private void SetStartPosition2(Vector2 screenPoint)
    {
        Vector2 anchoredPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, Camera.main, 
            out anchoredPosition);


        var width = parent.rect.width * screenPoint.x;
        var height = parent.rect.height * screenPoint.y;
        
        diamondIconRectTransform.anchorMax = Vector2.zero;
        diamondIconRectTransform.anchorMin = Vector2.zero;
        diamondIconRectTransform.transform.position = new Vector3(diamondIconRectTransform.transform.position.x,
            diamondIconRectTransform.transform.position.y, 0);
        diamondIconRectTransform.anchoredPosition = new Vector2(width, height);
    }
    
    private void SetStartPosition(Vector2 screenPoint)
    {
        Vector2 anchoredPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, Camera.main, 
            out anchoredPosition);
        
        diamondIconRectTransform.anchoredPosition = anchoredPosition;
    }
}
