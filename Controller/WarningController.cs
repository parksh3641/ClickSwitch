using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningController : MonoBehaviour
{
    public Image warning;

    private void Awake()
    {
        warning.gameObject.SetActive(false);
    }

    public void Hit()
    {
        warning.DOKill(true);
        warning.gameObject.SetActive(true);

        warning.DOFade(1, 0.5f).From(0).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            warning.gameObject.SetActive(false);
        });
    }
}
