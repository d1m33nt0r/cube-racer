using System.Collections;
using UnityEngine;
using DG.Tweening;
using Services.DiamondCountManager;
using UnityEngine.SceneManagement;
using Zenject;

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
    private DiamondCountManager diamondCountManager;
    
    public void Construct(DiamondCountManager diamondCountManager)
    {
        this.diamondCountManager = diamondCountManager;
    }
    
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
    
    public void CreateDiamondAndMoveWithDelay(Vector2 screenPoint, int countDiamonds)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        diamondIconRectTransform.GetComponent<DiamondIconController>().Construct(diamondCountManager, countDiamonds);
        SetStartPosition(screenPoint);
        diamondIconRectTransform.GetComponent<DiamondIconController>().Move(target);
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
        diamondIconRectTransform.anchorMax = Camera.main.ScreenToViewportPoint(screenPoint);
        diamondIconRectTransform.anchorMin = Camera.main.ScreenToViewportPoint(screenPoint);
        diamondIconRectTransform.anchoredPosition = Camera.main.ScreenToViewportPoint(screenPoint);
    }
}
