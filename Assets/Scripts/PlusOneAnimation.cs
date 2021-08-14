using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlusOneAnimation : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(IncreaseSizeAndRunFadeOut());
    }

    public IEnumerator IncreaseSizeAndRunFadeOut()
    {
        transform.DOScale(1.3f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeOutAndDestroy());

    }

    public IEnumerator FadeOutAndDestroy()
    {
        transform.DOScale(0.1f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.gameObject);
    }
}