using System;
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
            else
            {
                settingsButton.SetActive(true);
                closeButton.SetActive(false);
            }
        }
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
    
    private void SetStartPosition(Vector2 screenPoint)
    {
        Vector2 anchoredPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPoint, Camera.main, 
            out anchoredPosition);
        
        diamondIconRectTransform.anchoredPosition = anchoredPosition;
    }
}
