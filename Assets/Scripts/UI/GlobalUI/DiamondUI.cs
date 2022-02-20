using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Services.DiamondCountManager;
using UnityEngine.SceneManagement;
using Zenject;

public class DiamondUI : MonoBehaviour
{
    [SerializeField] private GameObject diamondIconPrefab;
    [SerializeField] private GameObject plusOnePrefab;
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform target;
    [SerializeField] public GameObject settingsButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private RectTransform panel;

    
    private RectTransform diamondIconRectTransform;
    private Canvas canvas;
    private RectTransform plusOneRectTransform;
    
    public RectTransform DiamondPanel => target;
    private DiamondCountManager diamondCountManager;

    public void Construct(DiamondCountManager diamondCountManager)
    {
        this.diamondCountManager = diamondCountManager;
    }
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
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

    private void OnActiveSceneChanged(Scene _current, Scene _next)
    {
        
    }

    public void DisableSettingsButton()
    {
        settingsButton.SetActive(false);
    }

    public void CreateDiamondAndMoveWithDelay(Vector2 screenPoint, int countDiamonds)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        diamondIconRectTransform.GetComponent<DiamondIconController>().Construct(diamondCountManager, countDiamonds);
        SetStartPosition(diamondIconRectTransform, screenPoint);
        diamondIconRectTransform.GetComponent<DiamondIconController>().Move(target);
    }

    public void CreateDiamond(Vector2 screenPoint)
    {
        diamondIconRectTransform = Instantiate(diamondIconPrefab, parent).GetComponent<RectTransform>();
        SetStartPosition(diamondIconRectTransform, screenPoint);
        
        var diamondIconController = diamondIconRectTransform.GetComponent<DiamondIconController>();

        diamondIconRectTransform.DOAnchorMax(target.anchorMax, 0.25f);
        diamondIconRectTransform.DOAnchorMin(target.anchorMin, 0.25f);
        
        panel.DOScale(1.2f, 0.1f).onComplete = (() =>
        {
            panel.DOScale(1f, 0.15f);
        });
        
        target.DOScale(1.4f, 0.1f).onComplete = (() =>
        {
            target.DOScale(1f, 0.15f);
        });
        
        diamondIconRectTransform
            .DOAnchorPos(target.anchoredPosition - new Vector2(5f, 15f), 0.25f)
            .OnComplete(diamondIconController.SetMovingDone);
    }
    
    private void SetStartPosition(RectTransform rectTransform, Vector2 screenPoint)
    {
        rectTransform.anchorMax = Camera.main.ScreenToViewportPoint(screenPoint);
        rectTransform.anchorMin = Camera.main.ScreenToViewportPoint(screenPoint);
        rectTransform.anchoredPosition = Camera.main.ScreenToViewportPoint(screenPoint);
    }
    
    private void SetStartPosition(List<RectTransform> rectTransform, List<Vector2> screenPoints)
    {
        for (var i = 0; i < screenPoints.Count; i++)
        {
            rectTransform[i].anchorMax = Camera.main.ScreenToViewportPoint(new Vector2(screenPoints[i].x + 130, screenPoints[i].y + 50));
            rectTransform[i].anchorMin = Camera.main.ScreenToViewportPoint(new Vector2(screenPoints[i].x + 130, screenPoints[i].y + 50));
            rectTransform[i].anchoredPosition = Camera.main.ScreenToViewportPoint(new Vector2(screenPoints[i].x + 130, screenPoints[i].y + 50));
        }
    }
    
    public void CreatePlusOneEffect(List<Vector2> screenPoints)
    {
        var transforms = new List<RectTransform>();
        var startScale = new Vector3(0.8f, 0.8f, 0.8f);
        
        for (var i = 0; i < screenPoints.Count; i++)
        {
            transforms.Add(Instantiate(plusOnePrefab, parent).GetComponent<RectTransform>());
            transforms.Last().localScale = startScale;
        }
        
        SetStartPosition(transforms, screenPoints);
        
        PlusOneEffectAnimation(transforms);
    }
    
    private async void PlusOneEffectAnimation(List<RectTransform> transforms)
    {
        var timeToUpScale = 0.25f;
        var timeToDownScale = 0.35f;
        
        var upScale = new Vector3(1.1f, 1.1f, 1.1f);
        var downScale = Vector3.zero;
        
        await ScaleUp(upScale, timeToUpScale, transforms);
        
        foreach (var trans in transforms)
            trans.DOScale(downScale, timeToDownScale);
    }

    private Task ScaleUp(Vector3 upScale, float timeToUpScale, List<RectTransform> transforms)
    {
        foreach (var trans in transforms)
            trans.DOScale(upScale, timeToUpScale);
        
        return Task.Delay(300);
    }
    
    private async void PlusOneEffectAnimation()
    {
        var timeToUpScale = 0.3f;
        var timeToDownScale = 0.5f;

        var upScale = new Vector3(1.2f, 1.2f, 1.2f);
        var downScale = Vector3.zero;

        await ScaleUp(upScale, timeToUpScale);
        plusOneRectTransform.DOScale(downScale, timeToDownScale);
    }

    private Task ScaleUp(Vector3 upScale, float timeToUpScale)
    {
        plusOneRectTransform.DOScale(upScale, timeToUpScale);
        return Task.Delay(300);
    }
}
