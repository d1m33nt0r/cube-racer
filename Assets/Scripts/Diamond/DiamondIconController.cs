using System.Collections;
using DG.Tweening;
using Services.DiamondCountManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DiamondIconController : MonoBehaviour
{
    private bool isMovingDone;
    private int countDiamonds;
    private DiamondCountManager diamondCountManager;
    private const string BALLOON = "Balloon";
    public void Construct(DiamondCountManager diamondCountManager, int countDiamonds)
    {
        this.diamondCountManager = diamondCountManager;
        this.countDiamonds = countDiamonds;
    }

    public void SetMovingDone()
    {
        isMovingDone = true;
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    public void SetDiamondCount(int count)
    {
        countDiamonds = count;
    }
    
    public void Move(RectTransform target)
    {
        StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(RectTransform target)
    {
        yield return new WaitForSeconds(Random.Range(1, 1.2f));
        
        var rectTransform = GetComponent<RectTransform>();
        
        rectTransform.DOAnchorMax(target.anchorMax, 1f);
        rectTransform.DOAnchorMin(target.anchorMin, 1f);
        
        rectTransform
            .DOAnchorPos(target.anchoredPosition - new Vector2(5f, 15f), 1f)
            .OnComplete(SetMovingDone);
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitUntil(() => isMovingDone);
        
        if (SceneManager.GetActiveScene().name == BALLOON)
        {
            diamondCountManager.UpdateData(diamondCountManager.GetDiamondCount() + countDiamonds);
            diamondCountManager.WriteData();
        }
        
        Destroy(gameObject);
    }
}
