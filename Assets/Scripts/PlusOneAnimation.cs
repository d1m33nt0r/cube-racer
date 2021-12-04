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
        transform.DOScale(1.5f, 0.02f);
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        transform.DOScale(0.1f, 0.025f);
        yield return new WaitForSeconds(0.025f);
        Destroy(transform.gameObject);
    }
}