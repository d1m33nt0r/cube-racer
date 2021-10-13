using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlusOneAnimation : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(IncreaseSizeAndRunFadeOut());
    }

    private IEnumerator IncreaseSizeAndRunFadeOut()
    {
        transform.DOScale(1.5f, 0.08f);
        yield return new WaitForSeconds(0.08f);
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        transform.DOScale(0.1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        Destroy(transform.gameObject);
    }
}