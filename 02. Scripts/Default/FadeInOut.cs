using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;

    public CanvasGroup canvasGroup;

    void Awake()
    {
        instance = this;
    }

    [Button]
    public void FadeIn()
    {
        StartCoroutine(Fade(true));
    }

    [Button]
    public void FadeOut()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        if (isFadeIn)
        {
            canvasGroup.alpha = 1;
            Tween tween = canvasGroup.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
            canvasGroup.gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
            Tween tween = canvasGroup.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
            StartCoroutine(Fade(true));
        }
    }

}